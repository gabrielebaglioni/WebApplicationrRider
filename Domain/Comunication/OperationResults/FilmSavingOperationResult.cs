using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Comunication.OperationResults;

public class FilmSavingOperationResult : OperationResult<FilmOutputDto>
{
    public FilmSavingOperationResult(
        bool ok,
        string? message = null,
        FilmOutputDto? result = default
    ) : base(ok, message, result)
    {
    }

    public static FilmSavingOperationResult Okay(FilmOutputDto? result, string? message = null)
    {
        return new FilmSavingOperationResult(true, message, result);
    }

    public static FilmSavingOperationResult Nok(string? message = null)
    {
        return new FilmSavingOperationResult(false, message);
    }
}