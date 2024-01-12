using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Request;
using Polly;
using Polly.Registry;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PersonalProject.InternalPortal.Services.Helpers;

public abstract class BaseRequestsClient<ILogCategory>
{
    private readonly IPolicyRegistry<string> _policyRegistry;
    private readonly ILogger<ILogCategory> _logger;

    protected BaseRequestsClient(IPolicyRegistry<string> policyRegistry, ILogger<ILogCategory> logger)
    {
        _policyRegistry = policyRegistry;
        _logger = logger;
    }

    public async Task HandleUnsucessfulHttpResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        string responseContentString = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            BadRequestException ex;
            try
            {
                var validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(responseContentString);
                string message = validationProblemDetails!.Title!;
                HttpStatusCode value = (HttpStatusCode)validationProblemDetails.Status!.Value;
                IDictionary<string, string[]> errors = validationProblemDetails.Errors;
                // for API validation however most validation is done on front end so may not be necessary
                ex = new BadRequestException(message, errors, value);

                _logger.LogError(ex, $"Bad response Recieved from {response.RequestMessage!.RequestUri}. Message: {message}");
            }
            catch (Exception)
            {
                throw new BadRequestException(responseContentString, response.StatusCode);
            }
            throw ex;
        }
        else if (response.StatusCode == HttpStatusCode.Conflict || response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new BadRequestException(responseContentString, response.StatusCode);
        }
        throw new Exception($"API call to {response.RequestMessage.RequestUri} failed with response content: {responseContentString}");
    }


    /// <summary> Performs HTTP GET request</summary>
    /// <typeparam name="TResponse">The type of object to deserialize from the response and return.</typeparam>
    /// <param name="httpClient">The HttpClient.</param>
    /// <param name="target">The URL resource path.</param>
    /// <param name="auditLogParams">Optional audit logging parameters</param>
    /// <param name="pollyParams">Optinal key for executing against a Polly policy.</param>
    /// <returns>A deserialized object</returns>
    public async Task<TResponse?> GetAsync<TResponse>(HttpClient httpClient, string target, AuditLogParameters? auditLogParams = null, PollyParemters? pollyParams = null)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, target);
        var responseObject = await PerformSendAsync<TResponse>(httpClient, request, auditLogParams, pollyParams);

        return responseObject;
    }

    /// <summary> Performs HTTP POST request without request body</summary>
    /// <typeparam name="TResponse">The type of object to deserialize from the response and return.</typeparam>
    /// <param name="httpClient">The HttpClient.</param>
    /// <param name="target">The URL resource path.</param>
    /// <param name="auditLogParams">Optional audit logging parameters</param>
    /// <param name="pollyParams">Optinal key for executing against a Polly policy.</param>
    /// <returns>A deserialized object</returns>
    public async Task<TResponse?> PostAsync<TResponse>(HttpClient httpClient, string target, AuditLogParameters? auditLogParams = null, PollyParemters? pollyParams = null)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, target);
        var responseObject = await PerformSendAsync<TResponse>(httpClient, request, auditLogParams, pollyParams);

        return responseObject;
    }

    /// <summary> Performs HTTP POST request with request body</summary>
    /// <typeparam name="TResponse">The type of object to deserialize from the response and return.</typeparam>
    /// <param name="httpClient">The HttpClient.</param>
    /// <param name="target">The URL resource path.</param>
    /// <param name="requestBody">The request body</param>
    /// <param name="auditLogParams">Optional audit logging parameters</param>
    /// <param name="pollyParams">Optinal key for executing against a Polly policy.</param>
    /// <returns>A deserialized object</returns>
    public async Task<TResponse?> PostAsync<TResponse, TRequest>(HttpClient httpClient, string target, TRequest? requestBody, AuditLogParameters? auditLogParams = null, PollyParemters? pollyParams = null)
    {
        var request = BuildRequestWithContent(HttpMethod.Post, target, requestBody);
        var responseObject = await PerformSendAsync<TResponse>(httpClient, request, auditLogParams, pollyParams);

        return responseObject;
    }

    /// <summary> Performs HTTP PUT request</summary>
    /// <typeparam name="TResponse">The type of object to deserialize from the response and return.</typeparam>
    /// <param name="httpClient">The HttpClient.</param>
    /// <param name="target">The URL resource path.</param>
    /// <param name="requestBody">The request body</param>
    /// <param name="auditLogParams">Optional audit logging parameters</param>
    /// <param name="pollyParams">Optinal key for executing against a Polly policy.</param>
    /// <returns>A deserialized object</returns>
    public async Task<TResponse?> PutAsync<TResponse, TRequest>(HttpClient httpClient, string target, TRequest? requestBody, AuditLogParameters? auditLogParams = null, PollyParemters? pollyParams = null)
    {
        var request = BuildRequestWithContent(HttpMethod.Put, target, requestBody);
        var responseObject = await PerformSendAsync<TResponse>(httpClient, request, auditLogParams, pollyParams);

        return responseObject;
    }

    /// <summary> Performs HTTP DELETE request</summary>
    /// <typeparam name="TResponse">The type of object to deserialize from the response and return.</typeparam>
    /// <param name="httpClient">The HttpClient.</param>
    /// <param name="target">The URL resource path.</param>
    /// <param name="requestBody">The request body</param>
    /// <param name="auditLogParams">Optional audit logging parameters</param>
    /// <param name="pollyParams">Optinal key for executing against a Polly policy.</param>
    /// <returns>A deserialized object</returns>
    public async Task<TResponse?> DeleteAsync<TResponse, TRequest>(HttpClient httpClient, string target, TRequest? requestBody, AuditLogParameters? auditLogParams = null, PollyParemters? pollyParams = null)
    {
        var request = BuildRequestWithContent(HttpMethod.Delete, target, requestBody);
        var responseObject = await PerformSendAsync<TResponse>(httpClient, request, auditLogParams, pollyParams);

        return responseObject;
    }

    public async Task<HttpResponseMessage> ExecuteClientTaskWithPolicyAsync(PollyParemters policyParams, Task<HttpResponseMessage> httpClientTask)
    {
        var context = policyParams.BuildPollyContext(_logger);

        if (_policyRegistry.TryGet<IAsyncPolicy<HttpResponseMessage>>(policyParams.PolicyKey, out var policy))
        {
            return await policy.ExecuteAsync(rp => httpClientTask, context);
        }
        else
        {
            throw new KeyNotFoundException($"Failed to find polly policy {policyParams.PolicyKey} for endpoint {context["source"]}");
        }
    }

    private T? TryParseJson<T>(string input)
    {
        try
        {
            var responseObject = JsonSerializer.Deserialize<T>(input);
            return responseObject;
        }
        catch
        {
            if (typeof(T) == typeof(string))
            {
                var responseString = (T)Convert.ChangeType(input, typeof(T));
                return responseString;
            }

            throw;
        }
    }

    private HttpRequestMessage BuildRequestWithContent<TRequest>(HttpMethod httpMethod, string target, TRequest? requestBody)
    {
        HttpContent? content;
        if (typeof(TRequest).IsAssignableTo(typeof(HttpContent)))
        {
            content = requestBody as HttpContent;
        }
        else
        {
            content = requestBody != null
                ? new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json") : null;
        }

        return new (httpMethod, target) { Content = content };
    }

    private async Task<TResponse?> PerformSendAsync<TResponse>(HttpClient httpClient, HttpRequestMessage request, AuditLogParameters? auditLogParams, PollyParemters? pollyParams = null)
    {
        TResponse? responseObject = default;
        HttpResponseMessage response;

        if (auditLogParams != null)
        {
            request.Headers.Add(AuditLogHeaders.Username, auditLogParams.Username);
            request.Headers.Add(AuditLogHeaders.UserType, auditLogParams.UserType);
            request.Headers.Add(AuditLogHeaders.EntityType, auditLogParams.EntityType);
            if (auditLogParams.Username != null)
                request.Headers.Add(AuditLogHeaders.EntityId, auditLogParams.Username);
        }

        try
        {
            if (pollyParams != null)
            {
                response = await ExecuteClientTaskWithPolicyAsync(pollyParams, httpClient.SendAsync(request));
            }
            else
            {
                response = await httpClient.SendAsync(request);
            }

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(responseString))
                {
                    responseObject = TryParseJson<TResponse>(responseString);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"API call to {request.RequestUri} failed. See inner exception for details", ex);
        }

        await HandleUnsucessfulHttpResponse(response);
        return responseObject;
    }
}