using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Models.Context;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly FilmContext _dbContext;

    public ActorsController(FilmContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/Actors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetActors()
    {
        if (!_dbContext.Actor.Any())
            return NotFound();

        var actors = await _dbContext.Actor
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.EarningSale)
            .ToListAsync();

        var actorsOutput = actors.Select(actor => (ActorOutputDto)actor).ToList();
        return Ok(actorsOutput);
    }

    // GET: api/Actors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorOutputDto>> GetActor(int id)
    {
        var actor = await _dbContext.Actor
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.EarningSale)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (actor == null) return NotFound();

        return (ActorOutputDto)actor;
    }

    // POST: api/Actors
    [HttpPost]
    public async Task<ActionResult<ActorOutputDto>> CrateActor(ActorSaveDto actorSaveDto)
    {
        if (await _dbContext.Actor.AnyAsync(a => a.Name == actorSaveDto.Name))
            return BadRequest("Actor already exists");
        var actor = (Actor)actorSaveDto;

        //recupero i film dal db che hanno lo stesso id di quelli passati di quelli passati in input
        var films = _dbContext.Films.AsEnumerable()
            .Where(f => actorSaveDto.Films
                .Any(dto => dto.Title == f.Title))
            .AsEnumerable();
        foreach (var film in films)
            actor.FilmsActor.Add(new ActorFilm
            {
                FkActor = actor.Id,
                FkFilm = film.Id,
                Film = film,
                Actor = actor
            });

        await _dbContext.Actor.AddAsync(actor);
        await _dbContext.SaveChangesAsync();

        var output = (ActorOutputDto)actor;
        // includo il genere e il totale guadagno del film
        foreach (var actorFilm in output.Films)
        {
            var film = await _dbContext.Films
                .Include(f => f.Genre)
                .Include(f => f.EarningSale)
                .FirstOrDefaultAsync(f => f.Id == actorFilm.Id);
            if (film == null) continue;
            actorFilm.GenreName = film.Genre?.Name ?? string.Empty;
            actorFilm.TotalEarning = film.EarningSale?.TotalEarning ?? 0;
        }

        return Ok(OperationResult.OK($"Attore con ID {actor.Id} Creato correttamente"));
    }

    // PUT: api/Actors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, ActorSaveDto actorSaveDto)
    {
        var actor = await _dbContext.Actor
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
            .ThenInclude(fa => fa.Film)
            .ThenInclude(film => film!.EarningSale)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (actor == null)
            return NotFound(OperationResult.NOK("attore non trovato"));
        if (actorSaveDto.Name != actor.Name)
            if (await _dbContext.Actor.AnyAsync(a => a.Name == actorSaveDto.Name))
                return BadRequest("Actor already exists");
        if (!actorSaveDto.Films
                .All(fDto => actor.FilmsActor
                    .Any(f => f.Film != null && f.Film.Title == fDto.Title))
            || !actor.FilmsActor
                .All(f => actorSaveDto.Films
                    .Any(fDto => f.Film != null && f.Film.Title == fDto.Title)))
        {
            actor.FilmsActor.Clear();
            var films = _dbContext.Films.AsEnumerable()
                .Where(f => actorSaveDto.Films
                    .Any(dto => dto.Title == f.Title))
                .AsEnumerable();
            foreach (var film in films)
                actor.FilmsActor.Add(new ActorFilm
                {
                    FkActor = actor.Id,
                    FkFilm = film.Id,
                    Film = film,
                    Actor = actor
                });
            //actor.FilmsActor.AddFilm(film);
        }

        //aggiorno i campi del db
        actor.Name = actorSaveDto.Name;
        actor.Surname = actorSaveDto.Surname;
        actor.Birthdate = actorSaveDto.Birthdate;
        await _dbContext.SaveChangesAsync();
        // var output = (ActorOutputDto)actor;
        return Ok(OperationResult.OK($"Attore con ID {actor.Id} aggiornato correttamente"));
    }

    // DELETE: api/Actors/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ActorOutputDto>> DeleteActor(int id)
    {
        var actor = await _dbContext.Actor.FindAsync(id);
        if (actor == null) return NotFound();

        _dbContext.Actor.Remove(actor);
        await _dbContext.SaveChangesAsync();

        return Ok(OperationResult.OK($"Attore con ID {actor.Id} eliminato correttamente"));
    }
}