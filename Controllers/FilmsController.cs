using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Incoming;
using WebApplicationrRider.Models.DTOs.Outgoing;
using WebApplicationrRider.Models.Entity;
using WebApplicationrRider.Utils;

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
            .Include(f => f.ActorsFilm)!
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
            .Include(f => f.ActorsFilm)!
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

        return Ok(output);
    }


    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, /*[FromBody]*/ FilmSaveDto filmSaveDto)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null)
            return NotFound();
        //cerco EarnigSale
        var earningsSale = await _dbContext.EarningSales.FirstOrDefaultAsync(e => e.FkFilm == film.Id);

        if (earningsSale == null)
        {
            earningsSale = new EarningSale
            {
                TotalEarning = filmSaveDto.TotalEarning
            };
            _dbContext.EarningSales.Add(earningsSale);
        }
        else
        {
            earningsSale.TotalEarning = filmSaveDto.TotalEarning;
        }

        if (filmSaveDto.Title != film.Title &&
            _dbContext.Films.Any(f => f.Title == filmSaveDto.Title && f.Id != filmSaveDto.Id))
            return BadRequest(OperationResult.NOK("Titolo esistente"));

        Genre genre = null;
        // controllo il valore del GenreName
        if (filmSaveDto.GenreName != film.Genre?.Name)
        {
            // cerco il genere nel db
            genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);
            if (genre == null) return BadRequest(OperationResult.NOK("il genere non esiste"));
        }


        film.Title = filmSaveDto.Title ?? string.Empty;
        film.ReleaseDate = filmSaveDto.ReleaseDate;
        film.Genre = genre ?? film.Genre;

        await _dbContext.SaveChangesAsync();


        var filmOutput = new FilmOutputDto
        {
            Id = film.Id,
            Title = film.Title,
            GenreName = film.Genre?.Name ?? string.Empty,
            TotalEarning = film.EarningSale?.TotalEarning ?? 0,
            ReleaseDate = film.ReleaseDate
        };

        return Ok(filmOutput);
    }


    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null) return Ok(OperationResult.NOK("Film inesistente "));

        _dbContext.Films.Remove(film);
        await _dbContext.SaveChangesAsync();
        return Ok(OperationResult.OK());
    }
}