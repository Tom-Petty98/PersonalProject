using Microsoft.OpenApi.Models;
using PersonalProject.Domain.Request;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PersonalProject.CoreAPI.Controllers.Helpers;

public class AuditHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        if (context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AuditLogFilterFactoryAttribute)))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuditLogHeaders.Username,
                Description = "The user's username",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = true
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuditLogHeaders.UserType,
                Description = "The microservice the request is from",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = true
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuditLogHeaders.EntityId,
                Description = "The PK of the entity. Required if not set in the query string",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = false
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AuditLogHeaders.EntityType,
                Description = "The type of entity being audited",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = true
            });
        }
    }
}
