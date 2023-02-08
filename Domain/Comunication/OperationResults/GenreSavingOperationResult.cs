using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Comunication.OperationResults;

public class GenreSavingOperationResult : OperationResult<GenreOutputDto>
{
    public GenreSavingOperationResult(
        bool ok,
        string? message = null,
        GenreOutputDto? result = default
    ) : base(ok, message, result)
    {
    }

    public static GenreSavingOperationResult Okay(GenreOutputDto? result, string? message = null)
    {
        return new GenreSavingOperationResult(true, message, result);
    }

    public static GenreSavingOperationResult Nok(string? message = null)
    {
        return new GenreSavingOperationResult(false, message);
    }
}