using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
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
    public async  Task<ActionResult<IEnumerable<FilmOutputDto>>> GetFilms(string? genreName)
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
        if ( await _dbContext.Films.AnyAsync(f => f.Title == filmSaveDto.Title))
            return BadRequest(OperationResult.NOK("A film with the same title already exists."));
        var genre = await _dbContext.Genres
            .FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);

        if (genre == null)
        {
            return BadRequest(OperationResult.NOK("genere non esisiset"));
        }
        var film = new Film
        {
            Title = filmSaveDto.Title,
            ReleaseDate = filmSaveDto.ReleaseDate
        };

        film.Genre = genre;
        // Creo un nuovo EarningSale per il nuovo film
        var newEarningSale = new EarningSale
        {
            TotalEarning = filmSaveDto.TotalEarning,
          
        };
        film.EarningSale = newEarningSale;

        var actors =  _dbContext.Actor.AsEnumerable()
            .Where(e => filmSaveDto.Actors.Any(dto => dto.Name == e.Name && dto.Surname == e.Surname))
            .AsEnumerable();

        foreach (var actor in actors)
        {
            film.AddActor(actor);
        }
        //actors.ForEach(actor => {film.AddActor(actor);});
        await _dbContext.Films.AddAsync(film);
        await _dbContext.SaveChangesAsync();
        
        FilmOutputDto output = (FilmOutputDto)film;
        //Se al momento della creazone un attore risulta inesistente, viene restituito un warning  ma il film viene comunque creato
        if (actors.Count() != filmSaveDto.Actors.Count())
        {
            
            return Ok(OperationResult.NOK(
                "Attenzione Film inserito parzialmente: alcuni attori non sono stati trovati nel database:" +
                $" {string.Join(", ", filmSaveDto.Actors
                    .Where(dto => !actors
                        .Any(a => a.Name == dto.Name && a.Surname == dto.Surname))
                    .Select(dto => $"Nome: {dto.Name}, Cognome: {dto.Surname}"))}"));
        }
        return Ok(output);
    }



    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmSaveDto filmSaveDto)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null)
            return NotFound(OperationResult.NOK("film non trovato"));
        //cerco EarningSale con id uguale a quello del film
        var earningSale = await _dbContext.EarningSales.FindAsync(film.Id);
        //se viene modificato EarningSale, lo aggiorno
        if (earningSale != null && earningSale.TotalEarning != filmSaveDto.TotalEarning)
        {
            earningSale.TotalEarning = filmSaveDto.TotalEarning;
        }
        
        Genre? genre = null;
        // controllo il valore del GenreName
        if (filmSaveDto.GenreName != film.Genre?.Name)
        {
            // cerco il genere nel db
            genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);
            
        }

        film.Title = filmSaveDto.Title ?? string.Empty;
        film.ReleaseDate = filmSaveDto.ReleaseDate;
        film.Genre = genre ?? film.Genre;
        film.EarningSale = earningSale ;
        
        var actors = _dbContext.Actor.AsEnumerable()
            .Where(a => filmSaveDto.Actors.Any(dto => dto.Name == a.Name && dto.Surname == a.Surname))
            .AsEnumerable();

        foreach (var actor in actors)
        {
            //controllo se l'attore esiste nel database
            if (actor == null)
            {
                return Ok(OperationResult.NOK(
                    $"Attore non trovato nel database: Nome: {actor.Name}, Cognome: {actor.Surname}"));
            }
            //se l'attore non è presente nel film, lo aggiungo
            if (!film.ActorsFilm.Any(a => a.Actor != null && a.Actor.Name == actor.Name && a.Actor.Surname == actor.Surname))
            {
                film.AddActor(actor);
            }
        }

          //rimuovo gli attori presenti nel film che non sono più presenti nella richiesta di modifica

        foreach (var actor in actors)
        {
            if (!filmSaveDto.Actors.Any(dto => dto.Name == actor.Name && dto.Surname == actor.Surname))
            {
                //rimuovo l'attore dal film e la relazione
                film.RemoveActor(actor);
                
            }
        }
        
        //controllo se almeno un attore è presente nel film
        if (actors.Count() == 0)
        {
            return Ok(OperationResult.NOK("Attenzione: non è stato inserito nessun attore"));
        }


        _dbContext.Films.Update(film);
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
        //var output = (FilmOutputDto)film;
        return Ok(OperationResult.OK("Film eliminato")); 
    }
    
}