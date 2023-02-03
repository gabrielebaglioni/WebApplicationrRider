using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.Context;
using WebApplicationrRider.Models.DTOs.Incoming;
using WebApplicationrRider.Models.DTOs.Outgoing;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilmsController : ControllerBase
{
    private readonly FilmContext _dbContext;

    public FilmsController(FilmContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/Films
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FilmOutputDto>>> GetFilms(string? genreName)
    {
        if (!_dbContext.Films.Any())
            return NotFound();


        IEnumerable<Film> films = await _dbContext.Films
            .Include(f => f.Genre)
            .Include(f => f.EarningSale)
            .Include(f => f.ActorsFilm)
            .ThenInclude(af => af.Actor)
            .ToListAsync();
        if (!string.IsNullOrEmpty(genreName)) films = films.Where(x => x.Genre != null && x.Genre.Name == genreName);
        /*films = !string.IsNullOrEmpty(genreName)
           ? films.Where(x => x.Genre != null && x.Genre.Name == genreName)
           : films;*/
        var filmsOutput = films.Select(film => (FilmOutputDto)film).ToList();

        return Ok(filmsOutput);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public ActionResult<FilmOutputDto> GetMovieById(int id)
    {
        var film = _dbContext.Films
            .Include(f => f.Genre)
            .Include(f => f.EarningSale)
            .Include(f => f.ActorsFilm)
                .ThenInclude(af => af.Actor)
            .FirstOrDefault(x => x.Id == id);


        if (film == null)
            return NotFound();

        var filmsOutput = (FilmOutputDto?)film;


        return Ok(filmsOutput);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<IActionResult> CreateFilm([FromBody] FilmSaveDto filmSaveDto)
    {
        if (await _dbContext.Films.AnyAsync(f => f.Title == filmSaveDto.Title))
            return BadRequest(OperationResult.NOK("A film with the same title already exists."));
        var genre = await _dbContext.Genres
            .FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);

        if (genre == null) return BadRequest(OperationResult.NOK("genere non esisiset"));

        var film = (Film)filmSaveDto;
        film.Genre = genre;


        var actors = _dbContext.Actor.AsEnumerable()
            .Where(e => filmSaveDto.Actors.Any(dto => dto.Name == e.Name && dto.Surname == e.Surname))
            .AsEnumerable();

        foreach (var actor in actors) film.AddActor(actor);

        //actors.ForEach(actor => {film.AddActor(actor);});
        await _dbContext.Films.AddAsync(film);
        await _dbContext.SaveChangesAsync();

        var output = (FilmOutputDto)film;
        //Se al momento della creazone un attore risulta inesistente, viene restituito un warning  ma il film viene comunque creato
        if (actors.Count() != filmSaveDto.Actors.Count())
            return Ok(OperationResult.NOK(
                "Attenzione Film inserito parzialmente: alcuni attori non sono stati trovati nel database:" +
                $" {string.Join(", ", filmSaveDto.Actors
                    .Where(dto => !actors
                        .Any(a => a.Name == dto.Name && a.Surname == dto.Surname))
                    .Select(dto => $"Nome: {dto.Name}, Cognome: {dto.Surname}"))}"));

        return Ok(output);
    }


    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmSaveDto filmSaveDto)
    {
        var film = await _dbContext.Films
            .Include(f => f.EarningSale)
            .Include(f => f.Genre)
            .Include(f => f.ActorsFilm)
            .ThenInclude(af => af.Actor)
            .FirstOrDefaultAsync(f => f.Id == id);
        if (film == null)
            return NotFound(OperationResult.NOK("film non trovato"));
        //se il titolo è diverso da quello passato nel body, lo aggiorno ma solo se non esiste già un film con quel titolo
        if (film.Title != filmSaveDto.Title)
            if (await _dbContext.Films.AnyAsync(f => f.Title == filmSaveDto.Title))
                return BadRequest(OperationResult.NOK("A film with the same title already exists."));

        Genre? genre = null;
        // controllo il valore del GenreName
        if (filmSaveDto.GenreName != film.Genre?.Name)
        {
            // cerco il genere nel db
            genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);
            // se non esiste, restituisco un errore
            if (genre == null)
                return BadRequest(OperationResult.NOK("genere non esisiset"));
        }

        if (!filmSaveDto.Actors
                .All(aDto => film.ActorsFilm
                    .Any(afe => afe.Actor != null && afe.Actor.Name == aDto.Name &&
                                afe.Actor.Surname == aDto.Surname))
            || !film.ActorsFilm
                .All(afe => afe.Actor != null && filmSaveDto.Actors
                    .Any(aDto => aDto.Name == afe.Actor.Name && aDto.Surname == afe.Actor.Surname)))
        {
            film.DeleteAllActors();
            var actors = _dbContext.Actor.AsEnumerable()
                .Where(e => filmSaveDto.Actors.Any(dto => dto.Name == e.Name && dto.Surname == e.Surname))
                .AsEnumerable();

            foreach (var actor in actors) film.AddActor(actor);
        }

        //aggiorno i campi del
        film.Title = filmSaveDto.Title;
        if (genre != null)
        {
            film.Genre = genre;
            film.FkGenre = genre.Id;
        }

        film.ReleaseDate = filmSaveDto.ReleaseDate;
        if (film.EarningSale != null) film.EarningSale.TotalEarning = filmSaveDto.TotalEarning;
        await _dbContext.SaveChangesAsync();
        var output = (FilmOutputDto)film;
        return Ok(output);
    }


    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null) return Ok(OperationResult.NOK("Film inesistente "));

        _dbContext.Films.Remove(film);
        await _dbContext.SaveChangesAsync();
        return Ok(OperationResult.OK("Film eliminato"));
    }
}