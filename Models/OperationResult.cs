namespace WebApplicationrRider.Models;

public class OperationResult
{
    public bool Ok { get; set; }
    public string? Message { get; set; }
    
    public OperationResult(bool ok, string? message = null)
    {
        Ok = ok;
        Message = message;
    }

    public static OperationResult OK(string? message = null)
        => new OperationResult(true, string.IsNullOrEmpty(message) ? OkMessage : message );
    
    public static OperationResult NOK(string? message = null)
        => new OperationResult(false, string.IsNullOrEmpty(message)? NokMessage : message);

    private const string OkMessage = "Operzione eseguita";
    private const string NokMessage = "Operzione Non Eseguita";

}