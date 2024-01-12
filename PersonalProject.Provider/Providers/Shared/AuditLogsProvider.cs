using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Data;

namespace PersonalProject.Provider.Providers.Shared;

public interface IAuditLogsProvider
{
    Task<AuditLog> CreateAuditLogAsync(AuditLog auditLog);
    Task<AuditLog> UpdateAuditLogAsync(AuditLog auditLog);
    Task<AuditLog?> GetAuditLogById(int auditLogId);
}

public class AuditLogsProvider : IAuditLogsProvider
{
    private readonly ApplicationDbContext _context;
    public AuditLogsProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<AuditLog> CreateAuditLogAsync(AuditLog auditLog)
    {
        throw new NotImplementedException();
    }

    public Task<AuditLog?> GetAuditLogById(int auditLogId)
    {
        throw new NotImplementedException();
    }

    public Task<AuditLog> UpdateAuditLogAsync(AuditLog auditLog)
    {
        throw new NotImplementedException();
    }
}
