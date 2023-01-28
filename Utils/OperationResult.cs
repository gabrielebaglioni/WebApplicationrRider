namespace WebApplicationrRider.Utils;

public class OperationResult
{
    private const string OkMessage = "Operzione eseguita";
    private const string NokMessage = "Operzione Non Eseguita";

    public OperationResult(bool ok, string? message = null)
    {
        Ok = ok;
        Message = message;
    }

    public bool Ok { get; set; }
    public string? Message { get; set; }

    public static OperationResult OK(string? message = null)
    {
        return new(true, string.IsNullOrEmpty(message) ? OkMessage : message);
    }

    public static OperationResult NOK(string? message = null)
    {
        return new(false, string.IsNullOrEmpty(message) ? NokMessage : message);
    }
}