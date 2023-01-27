using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
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
    public ActionResult<IEnumerable<FilmOutputDto>> GetAllFilms()
    {
        if (!_dbContext.Films.Any())
            return NotFound();

        var allFilms = _dbContext.Films.Include("Genre").Include("EarningSale").ToList();
        var filmOutput = allFilms.Select(f => (FilmOutputDto?)f);
        return Ok(filmOutput);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public ActionResult<FilmOutputDto> GetMovieById(int id)
    {
        var film = _dbContext.Films
            .Include("Genre")
            .Include("EarningSale")
            .FirstOrDefault(x => x.Id == id);
        if (film == null)
            return NotFound();

        var filmsOutput = (FilmOutputDto?)film;
       


        return Ok(filmsOutput);
    }
    // GET: api/Genres/5
    // GET ALL FILMS BY GENRE
    [HttpGet("Genre{Name}")]
    public async Task<ActionResult<List<FilmOutputDto>>> GetFilmsByGenre(string genreName)
    {
        var filmsByGenre = await _dbContext.Films
            .Where(x => x.Genre != null && x.Genre.Name == genreName)
            .Include("EarningSale")
            .ToListAsync();
        var output = filmsByGenre.Select(f => (FilmOutputDto)f!);


        return Ok(output);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<FilmOutputDto>> CreateFilm(FilmSaveDto userData)
    {
        // cerco il genere nel db
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == userData.GenreName);
        var earningSale =
            await _dbContext.EarningSales.FirstOrDefaultAsync(es => es.SaleAmount* es.PriceSingleSale == userData.TotalEaring );

        if (genre == null)
        {
            return BadRequest("Invalid genre name");
        }

        var newFilm = new Film
        {
            Id = userData.Id,
            Title = userData.Title ?? string.Empty,
            ReleaseDate = userData.ReleaseDate,
            Genre = genre,
            EarningSale = earningSale ?? throw new InvalidOperationException("The earnig can't be null")
        };

        _dbContext.Add(newFilm);
        await _dbContext.SaveChangesAsync();

        var output = (FilmOutputDto?)newFilm;
        return Ok(output);
    }



    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmSaveDto userData)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null)
            return NotFound();
        //cerco EarnigSale
        var earningSale =
            await _dbContext.EarningSales.FirstOrDefaultAsync(es =>
                es.SaleAmount * es.PriceSingleSale == userData.TotalEaring);

        film.Title = userData.Title ?? string.Empty;
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

        if (earningSale != null) 
            film.EarningSale = earningSale;

        await _dbContext.SaveChangesAsync();

        var filmForOutputDto = new FilmOutputDto
        {
            Id = film.Id,
            Title = film.Title,
            GenreName = film.Genre?.Name ?? string.Empty,
            TotalEaring = film.EarningSale.PriceSingleSale * film.EarningSale.SaleAmount,
            ReleaseDate = film.ReleaseDate
        };

        return Ok(filmForOutputDto);
    }

    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var film = await _dbContext.Films.FindAsync(id);
        if (film == null) return Ok(OperationResult.NOK("Film inesistente => Id sbagliato"));

        _dbContext.Films.Remove(film);
        await _dbContext.SaveChangesAsync();
        return Ok(OperationResult.OK());
    }
    
}