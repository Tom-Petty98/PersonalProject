namespace DocumentsAPI.Models;

public class Document
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string? AzureStorageUri { get; set; }
    public string? MimeType { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? DeletedDateTime { get; set; }
    public DateTime? ExpiryDateTime { get; set; }
}
