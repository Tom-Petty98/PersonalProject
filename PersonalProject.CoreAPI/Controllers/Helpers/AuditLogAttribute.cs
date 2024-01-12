using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PersonalProject.CoreAPI.Services.Shared;
using PersonalProject.Domain.Request;
using System.Text;

namespace PersonalProject.CoreAPI.Controllers.Helpers;

public class AuditLogAttribute : ActionFilterAttribute
{
    private readonly IAuditLogsService _auditLogsService;
    public const string EntityIdHttpContextKey = "EntityId";

    public bool LogResponse { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    public AuditLogAttribute(IAuditLogsService auditLogsService)
    {
        _auditLogsService = auditLogsService ?? throw new ArgumentNullException(nameof(auditLogsService));
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var auditLogId = await PerformLogRequestAsync(context.HttpContext);

        if (LogResponse)
        {
            var response = context.Result as IStatusCodeActionResult ?? throw new InvalidCastException("Could not get result as object result");
            var statusCode = response.StatusCode ?? StatusCodes.Status500InternalServerError;

            await _auditLogsService.LogResponseAsync(auditLogId, statusCode);
        }

        await next();
    }

    public async Task<int> PerformLogRequestAsync(HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(Message))
            throw new InvalidOperationException("A Message is Required");

        var requestHeaders = httpContext.Request.Headers;

        var username = requestHeaders[AuditLogHeaders.Username].FirstOrDefault() ??
            throw new InvalidOperationException($"{AuditLogHeaders.Username} header is required for this request");

        var usertype = requestHeaders[AuditLogHeaders.UserType].FirstOrDefault() ??
            throw new InvalidOperationException($"{AuditLogHeaders.UserType} header is required for this request");

        var entityType = requestHeaders[AuditLogHeaders.EntityType].FirstOrDefault() ??
            throw new InvalidOperationException($"{AuditLogHeaders.EntityType} header is required for this request");

        var entityId = ReadEntityId(httpContext);
        var requestBody = await ReadRequestBody(httpContext.Request);

        int auditLogId = await _auditLogsService.LogRequestAsync(username, usertype, entityId, entityType, Message, requestBody);
        return auditLogId;
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        var requestBody = string.Empty;
        request.Body.Position = 0;

        using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        request.Body.Position = 0;
        return requestBody;
    }

    private int? ReadEntityId(HttpContext httpContext)
    {
        var queryApplicationId = httpContext.Request.Query["applictionId"];
        var queryInstallerId = httpContext.Request.Query["installerId"];
        var headerEntityId = httpContext.Request.Headers[AuditLogHeaders.EntityId];
        var itemsEntityId = httpContext.Items[EntityIdHttpContextKey];

        int entityId;

        if ((queryApplicationId.Count == 1 && int.TryParse(queryApplicationId.First(), out entityId)) ||
            (queryInstallerId.Count == 1 && int.TryParse(queryInstallerId.First(), out entityId)) ||
            (headerEntityId.Count == 1 && int.TryParse(headerEntityId.First(), out entityId)))
        {
            return entityId;
        }
        else if (itemsEntityId != null && int.TryParse(itemsEntityId.ToString(), out entityId))
        {
            return entityId;
        }
        else 
        { 
            return null!; 
        }
    }
}

