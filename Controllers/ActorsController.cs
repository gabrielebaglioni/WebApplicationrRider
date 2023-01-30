using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Incoming;
using WebApplicationrRider.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly FilmContext _context;

    public ActorsController(FilmContext context)
    {
        _context = context;
    }

    // GET: api/Actors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetActors()
    {
        var actors = await _context.Actor.ToListAsync();
        return actors.Select(a => (ActorOutputDto)a).ToList();
    }

    // GET: api/Actors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorOutputDto>> GetActor(int id)
    {
        var actor = await _context.Actor.FindAsync(id);

        if (actor == null)
        {
            return NotFound();
        }

        return (ActorOutputDto)actor;
    }

    // PUT: api/Actors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, ActorSaveDto actorSaveDto)
    {
        if (id != actorSaveDto.Id)
        {
            return BadRequest();
        }

        var actor = (Actor)actorSaveDto;
        _context.Entry(actor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ActorExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Actors
    [HttpPost]
    public async Task<ActionResult<ActorOutputDto>> AddActor(ActorSaveDto actorSaveDto)
    {
        var actor = (Actor)actorSaveDto;
        _context.Actor.Add(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetActor", new { id = actor.Id }, (ActorOutputDto)actor);
    }

    // DELETE: api/Actors/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ActorOutputDto>> DeleteActor(int id)
    {
        var actor = await _context.Actor.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }

        _context.Actor.Remove(actor);
        await _context.SaveChangesAsync();

        return (ActorOutputDto)actor;
    }

    private bool ActorExists(int id)
    {
        return _context.Actor.Any(e => e.Id == id);
    }
}


