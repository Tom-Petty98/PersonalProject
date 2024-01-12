using System.Net;

namespace PersonalProject.Domain.Request;

public class BadRequestException : Exception
{
    public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.BadRequest;
    private IDictionary<string, string[]> _errors = new Dictionary<string, string[]>();
    public IDictionary<string, string[]> Errors => _errors;
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception? innerException) : base(message, innerException) { }
    public BadRequestException(string message, HttpStatusCode statusCode) : this(message)
    {
        StatusCode = statusCode;
    }
    public BadRequestException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        _errors = errors;
    }

    public BadRequestException(string message, IDictionary<string, string[]> errors, HttpStatusCode statusCode) : this(message, statusCode)
    {
        _errors = errors;
    }
    //protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}