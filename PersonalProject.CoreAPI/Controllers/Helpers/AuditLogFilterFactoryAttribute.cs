using Microsoft.AspNetCore.Mvc.Filters;
using PersonalProject.Domain.Entities;

namespace PersonalProject.CoreAPI.Controllers.Helpers;

public class AuditLogFilterFactoryAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    /// <summary>
    /// Enables response logging. Enabled by default.
    /// </summary>
    public bool LogResponse { get; set; } = true;

    /// <summary>
    /// A message to save with the log event
    /// </summary>
    public string Message { get; set; } = string.Empty;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var auditLogFilter = serviceProvider.GetService<AuditLogAttribute>() ??
            throw new NullReferenceException("Could not find audit log attribute");

        auditLogFilter.LogResponse = LogResponse;
        auditLogFilter.Message = Message;

        return auditLogFilter;
    }
}
