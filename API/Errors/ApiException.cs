namespace API.Errors;

public class ApiException : ApiResponse
{
    // statusCode The HTTP status code of the exception.
    // message The error message associated with the exception.
    // details Additional details or context for the exception.
    public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
    {
        Details = details;
    }
    //Gets or sets additional details or context for the exception.
    public string Details { get; set; }
}
