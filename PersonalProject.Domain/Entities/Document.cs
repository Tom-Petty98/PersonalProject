﻿namespace PersonalProject.Domain.Entities;
public class Document
{
    public enum DocumentEntityType
    {
        Application,
        Installer
    }

    public int Id { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string? AzureDocumentId { get; set; }
    /// <summary>
    /// The primary key of the Entity this document is attached to
    /// </summary>
    public int? EntityId { get; set; }
    public DocumentEntityType EntityTypeId { get; set; }
    public int DocumentTypeId { get; set; }
    public long FileSizeBytes { get; set; }
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Email of user who create the record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }

    public virtual DocumentType? DocumentType { get; set; }
}