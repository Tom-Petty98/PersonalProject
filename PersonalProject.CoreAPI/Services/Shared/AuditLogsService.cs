using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Enums;
using PersonalProject.Provider.Providers.Shared;

namespace PersonalProject.CoreAPI.Services.Shared;

public interface IAuditLogsService
{
    /// <summary>
    /// Stores the details of an API request in the audit log.
    /// </summary>
    /// <param name="entityId">The primary key of the entity being updated</param>
    /// <param name="entityType">The enum int value for the entity being updated</param>
    /// <returns></returns>
    Task<int> LogRequestAsync(string username, string userType, int? entityId, string entityType, string message, string requestBody);

    Task<int?> LogResponseAsync(int auditLogId, int httpResponseCode);
}

public class AuditLogsService : IAuditLogsService
{
    private readonly IAuditLogsProvider _auditLogsProvider;
    public async Task<int> LogRequestAsync(string username, string userType, int? entityId, string entityType, string message, string requestBody)
    {
        if (!Enum.TryParse<AuditLogUserType>(userType, true, out var userTypeEnumValue))
        {
            throw new InvalidOperationException("User type must be Internal, Consent, External.");
        }

        if (!Enum.TryParse<AuditLogEntityType>(entityType, true, out var entityTypeEnumValue))
        {
            throw new InvalidOperationException("Entity type must be Installer, Application.");
        }

        var auditLog = new AuditLog
        {
            CreatedBy = username,
            UserType = userTypeEnumValue,
            EntityId = entityId,
            EntityType = entityTypeEnumValue,
            UpdateMethodMessage = message,
            Payload = requestBody,
            EventTimeStamp = DateTime.UtcNow
        };

        var savedAuditLog = await _auditLogsProvider.CreateAuditLogAsync(auditLog);
        return savedAuditLog.Id;
    }

    public async Task<int?> LogResponseAsync(int auditLogId, int httpResponseCode)
    {
        var auditLog = await _auditLogsProvider.GetAuditLogById(auditLogId);

        if (auditLog == null)
        {
            return null;
        }

        auditLog.ResultStatus = httpResponseCode;
        var updatedAuditLog = await _auditLogsProvider.UpdateAuditLogAsync(auditLog);
        return updatedAuditLog.Id;
    }
}
