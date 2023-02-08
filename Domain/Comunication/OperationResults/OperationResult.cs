namespace WebApplicationrRider.Domain.Comunication.OperationResults;

public class OperationResult
{
    protected const string OkMessage = "Operzione eseguita";
    protected const string NokMessage = "Operzione Non Eseguita";

    public OperationResult(bool ok, string? message = null)
    {
        Ok = ok;
        Message = message;
    }


    public bool Ok { get; set; }
    public string? Message { get; set; }

    public static OperationResult OK(string? message = null)
    {
        return new OperationResult(true, string.IsNullOrEmpty(message) ? OkMessage : message);
    }

    public static OperationResult NOK(string? message = null)
    {
        return new OperationResult(false, string.IsNullOrEmpty(message) ? NokMessage : message);
    }
}

public class OperationResult<T> : OperationResult
{
    public OperationResult(bool ok, string? message = null, T? result = default) : base(ok, message)
    {
        Result = result;
    }

    public T? Result { get; set; }

    public static OperationResult<T> OK(T? result, string? message = null)
    {
        return new OperationResult<T>(true, string.IsNullOrEmpty(message) ? OkMessage : message, result);
    }

    public static OperationResult<T> NOK(T? result, string? message = null)
    {
        return new OperationResult<T>(false, string.IsNullOrEmpty(message) ? NokMessage : message, result);
    }
}