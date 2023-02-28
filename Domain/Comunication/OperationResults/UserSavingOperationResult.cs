using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Comunication.OperationResults;

public class UserSavingOperationResult : OperationResult<UserOutputDto>
{
    public UserSavingOperationResult(
        bool ok,
        string? message = null,
        UserOutputDto? result = default
    ) : base(ok, message, result)
    {
    }

    public static UserSavingOperationResult Okay(UserOutputDto? result, string? message = null)
    {
        return new UserSavingOperationResult(true, message, result);
    }

    public static UserSavingOperationResult Nok(string? message = null)
    {
        return new UserSavingOperationResult(false, message);
    }
}