using System.Net;
using Newtonsoft.Json;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Exceptions;

namespace WebApplicationrRider.MiddleWhere;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CheckException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = new { success = false, message = ex.Message };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = new { success = false, message = "Errore generico: " + ex.Message };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}   
