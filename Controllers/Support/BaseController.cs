using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Exceptions;

namespace WebApplicationrRider.Controllers.Support;

public class BaseController : ControllerBase
{
    protected async Task<ActionResult> TryCatch(Func<Task<ActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (CheckException e)
        {
            return BadRequest(OperationResult.NOK(e.Message));
        }
        catch (Exception e)
        {
            return BadRequest(OperationResult.NOK("si è manifestato un errore generico: " + e.Message));
        }
    }
}