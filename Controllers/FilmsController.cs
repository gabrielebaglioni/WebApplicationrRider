
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Incoming;
using WebApplicationrRider.Models.DTOs.Outgoing;
using WebApplicationrRider.Utils;

namespace WebApplicationrRider.Controllers
{
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
    public ActionResult<IEnumerable<FilmOutputDto>> GetFilms(string? genreName)
    {
        if (!_dbContext.Films.Any())
            return NotFound();

        IEnumerable<Film>films  = _dbContext.Films.Include(f => f.Genre)
            .Include(f=>f.EarningSale).AsQueryable();
        if (!string.IsNullOrEmpty(genreName))
        {
            films = films.Where(x => x.Genre != null && x.Genre.Name == genreName);
        }

        /*films = !string.IsNullOrEmpty(genreName)
            ? films.Where(x => x.Genre != null && x.Genre.Name == genreName)
            : films;*/
        films = films.ToList();
        
        var outputs = films.Select(f => (FilmOutputDto?)f);
        return Ok(outputs);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public ActionResult<FilmOutputDto> GetMovieById(int id)
    {
        var film = _dbContext.Films
            .Include(f=>f.Genre)
            .Include(f=>f.EarningSale)
            .FirstOrDefault(x => x.Id == id);
            
            
        if (film == null)
            return NotFound();

        var filmsOutput = (FilmOutputDto?)film;
       


        return Ok(filmsOutput);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<FilmOutputDto>> CreateFilm(FilmSaveDto filmSaveDto)
    {
        if (_dbContext.Films.Any(f=>f.Title == filmSaveDto.Title))
        {
            return BadRequest(OperationResult.NOK("Titolo esistente"));
        }

        // Cerco il genere nel db
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == filmSaveDto.GenreName);

        if (genre == null)
        {
            return BadRequest("Invalid genre name");
        }

        var newFilm = (Film)filmSaveDto;
        newFilm.Genre = genre;

        // Creo un nuovo EarningSale per il nuovo film
        var newEarningSale = new EarningSale
        {
            TotalEarning = filmSaveDto.TotalEarning,
          
        };

        // Imposto il nuovo EarningSale per il nuovo film
        newFilm.EarningSale = newEarningSale;

        _dbContext.Add(newFilm);
        await _dbContext.SaveChangesAsync();

        var output = (FilmOutputDto?)newFilm;
        return Ok(output);
    }




    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(int id, [FromBody] FilmSaveDto filmSaveDto)
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
                Id = film.Id,
                TotalEarning = filmSaveDto.TotalEarning
            };
            _dbContext.EarningSales.Add(earningsSale);
        }
        else
        {
            earningsSale.TotalEarning = filmSaveDto.TotalEarning;
        }

        if (filmSaveDto.Title != film.Title && _dbContext.Films.Any(f=>f.Title == filmSaveDto.Title && f.Id!= filmSaveDto.Id))
        {
            return BadRequest(OperationResult.NOK("Titolo esistente"));
        }
        Genre? genre = null;
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

   
        var filmForOutputDto = new FilmOutputDto
        {
            Id = film.Id,
            Title = film.Title,
            GenreName = film.Genre?.Name ?? string.Empty,
            TotalEarning = film.EarningSale?.TotalEarning ?? 0,
            ReleaseDate = film.ReleaseDate
        };

        return Ok(filmForOutputDto);
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
}
