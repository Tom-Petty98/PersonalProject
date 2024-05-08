using DocumentsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentsService _documentsService;

    public DocumentController(IDocumentsService documentsService)
    {
        _documentsService = documentsService;
    }

    [HttpGet("{documentId:guid}", Name = "GetDocument")]
    [ProducesResponseType(typeof(IFormFile), 200)]
    public async Task<IActionResult> Get(Guid documentId)
    {
        var formFile = await _documentsService.DownloadDocumentAsync(documentId);
        return File(formFile.OpenReadStream(), formFile.ContentType, formFile.FileName);
    }

    [HttpPost("{expiryDateTime:datetime?}")]
    [ProducesResponseType(typeof(Guid), 201)]
    public async Task<IActionResult> Post(DateTime? expiryDateTime = null)
    {
        var documentId = await _documentsService.UploadDocumentAsync(Request.Form.Files[0], expiryDateTime);
        return CreatedAtRoute("GetDocument", new { documentId }, documentId);
    }

    [HttpPost("RemoveExpiryDate")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<IActionResult> RemoveExpiryDate([FromBody] List<Guid> documentIds)
    {
        await _documentsService.RemoveExpiryDateAsync(documentIds);
        return NoContent();
    }

    [HttpDelete("{documentId:guid}")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<IActionResult> Delete(Guid documentId)
    {
        await _documentsService.DeleteDocumentAsync(documentId);
        return NoContent();
    }

    [HttpPost("CleanUp")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<IActionResult> CleanUp()
    {
        await _documentsService.DeleteExpiredDocuments();
        return NoContent();
    }
}
