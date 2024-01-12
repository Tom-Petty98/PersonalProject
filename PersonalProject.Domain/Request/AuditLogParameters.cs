

namespace PersonalProject.Domain.Request;

public record AuditLogParameters
{
    public string Username { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int EntitiyId { get; set; } 
}

public static class AuditLogHeaders
{
    public const string Username = "X-Audit-Username";
    public const string UserType = "X-Audit-UserType";
    public const string EntityType = "X-Audit-EntityType";
    public const string EntityId = "X-Audit-EntityId";
}