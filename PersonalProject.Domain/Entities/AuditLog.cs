
using PersonalProject.Domain.Enums;

namespace PersonalProject.Domain.Entities;

/// <summary>
/// Update history of installer/application details
/// </summary>
public class AuditLog
{
    public int Id { get; set; }
    /// <summary>
    /// Primary key of the Enitity being updated
    /// </summary>
    public int? EntityId { get; set; }
    public AuditLogEntityType EntityType { get; set; }
    /// <summary>
    /// The microservice the request is from
    /// </summary>
    public AuditLogUserType UserType { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdateMethodMessage { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public int? ResultStatus { get; set; } 
    public DateTime EventTimeStamp { get; set; }
}
