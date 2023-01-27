using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;

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
    public ActionResult<IEnumerable<FilmForOutputDTO>> GetAllFilms()
    {
        if (!_dbContext.Films.Any())
            return NotFound();

        var allFilms = _dbContext.Films.Include("Genre").ToList();
        var _filmsDtos = allFilms.Select(f => (FilmForOutputDTO)f);
        return Ok(_filmsDtos);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public ActionResult<FilmForOutputDTO> GetMovieById(int id)
    {
        var film = _dbContext.Films
            .Include("Genre")
            .FirstOrDefault(x => x.Id == id);
        if (film == null)
            return NotFound();

        var _filmsDtos = (FilmForOutputDTO?)film;
       


        return Ok(_filmsDtos);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<FilmForOutputDTO>> CreateFilm(FilmSaveDTO UserData)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == UserData.GenreName);

        if (genre == null)
        {
            return BadRequest("Invalid genre name");
        }

        var newFilm = new Film
        {
            Id = UserData.Id,
            Title = UserData.Title,
            ReleaseDate = UserData.ReleaseDate,
            Genre = genre
        };

        _dbContext.Add(newFilm);
        await _dbContext.SaveChangesAsync();

        var output = (FilmForOutputDTO)newFilm;
        return Ok(output);
    }



    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmSaveDTO userData)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null)
            return NotFound();

        film.Title = userData.Title;
        film.ReleaseDate = userData.ReleaseDate;

        // controllo il valore del GenreName
        if (userData.GenreName != film.Genre?.Name)
        {
            // cerco il genere nel db
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == userData.GenreName);

            // se non esiste lo creo
            if (genre == null)
            {
                genre = new Genre { Name = userData.GenreName };
                _dbContext.Genres.Add(genre);
            }

            film.Genre = genre;
        }

        await _dbContext.SaveChangesAsync();

        var filmForOutputDTO = new FilmForOutputDTO
        {
            Id = film.Id,
            Title = film.Title,
            ReleaseDate = film.ReleaseDate,
            GenreName = film.Genre?.Name ?? string.Empty
        };

        return Ok(filmForOutputDTO);
    }

    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        if (_dbContext == null) return NotFound(OperationResult.NOK());

        var film = await _dbContext.Films.FindAsync(id);
        if (film == null) return NotFound();

        _dbContext.Films.Remove(film);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
    
}