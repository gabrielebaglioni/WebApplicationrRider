using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Controllers.Support;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Services;


namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : BaseController
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    
    // GET: api/Actors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetActors()
    {
       var actors = await _actorService.GetListAsync();
         return Ok(actors);
    }

    // GET: api/Actors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorOutputDto>> GetActor(int id)
    {
        var actor = await _actorService.Get(id);
        return Ok(actor);
    }

    // POST: api/Actors
    [HttpPost]
    public async Task<ActionResult<OperationResult>> CrateActor(ActorSaveDto actorSaveDto)
    {
        var result = await _actorService.CreateAsync(actorSaveDto);
        return Ok(ActorSavingOperationResult.OK(result, "attore aggiunto con successo nel database."));
    }

    // PUT: api/Actors/5
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> UpdateActor(int id, ActorSaveDto actorSaveDto)
    {
        var result = await _actorService.UpdateAsync(id, actorSaveDto);
        return Ok(ActorSavingOperationResult.OK(result, "attore aggiornato con successo nel database."));
    }

    // DELETE: api/Actors/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> DeleteActor(int id)
    {
        var result = await _actorService.DeleteAsync(id);
        return Ok(ActorSavingOperationResult.OK(result, "attore eliminato con successo nel database."));
    }
}