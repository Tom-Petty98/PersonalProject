namespace PersonalProject.Domain.Enums;

public enum AuditLogEntityType
{
    Installer = 0,
    Application = 1,
}

public enum AuditLogUserType
{
    Internal = 0,
    Consent = 1,
    External = 2
}
