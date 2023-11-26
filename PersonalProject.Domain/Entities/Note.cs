namespace PersonalProject.Domain.Entities;

public class Note
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// The primary key of the Entity this document is attached to
    /// </summary>
    public int? EntityId { get; set; }
    public bool IsInternalOnly { get; set; }
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Email of user who create the record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
