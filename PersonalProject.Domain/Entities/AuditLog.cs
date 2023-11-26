
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
    public int EntityId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdateMethodMessage { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime EventTimeStamp { get; set; }
}
