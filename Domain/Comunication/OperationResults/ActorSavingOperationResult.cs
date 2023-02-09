using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Comunication.OperationResults;

public class ActorSavingOperationResult : OperationResult<ActorOutputDto>
{
    public ActorSavingOperationResult(
        bool ok,
        string? message = null,
        ActorOutputDto? result = default
    ) : base(ok, message, result)
    {
    }
    public static ActorSavingOperationResult Okay(ActorOutputDto? result, string? message = null)
    {
        return new ActorSavingOperationResult(true, message, result);
    }
    public static ActorSavingOperationResult Nok(string? message = null)
    {
        return new ActorSavingOperationResult(false, message);
    }
}