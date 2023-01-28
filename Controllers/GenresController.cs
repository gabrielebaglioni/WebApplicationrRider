using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Incoming;
using WebApplicationrRider.Models.DTOs.Outgoing;
using WebApplicationrRider.Utils;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly FilmContext _dbContext;


    public GenresController(FilmContext dbContext)
    {
        _dbContext = dbContext;
    }

    //GET: api/Genres
    [HttpGet]
    public ActionResult<IEnumerable<GenreSaveDto>> GetAllGenres()
    {
        var allGenre = _dbContext.Genres.ToList();
        var genreOutput = allGenre.Select(g => (GenreSaveDto)g);
        return Ok(genreOutput);
    }

    

    //GET: api/Genres/5
    //GET A SINGLE GENRE BY ID
    [HttpGet("{id}")]
    public ActionResult<GenreSaveDto> GetGenreById(int id)
    {
        var genre = _dbContext.Genres
            .FirstOrDefault(x => x.Id == id);
        if (genre == null)
            return NotFound();

        var output = (GenreSaveDto)genre;
        return Ok(output);
    }

    //POST: api/Genres
    [HttpPost]
    public async Task<ActionResult<List<FilmOutputDto>>> PostGenre(GenreSaveDto userData)
    {
        var genre = _dbContext.Genres.Any(genres => genres.Name.Equals(userData.Name));
        if (genre)
            return BadRequest(OperationResult.NOK("Genere già Inesistente"));
        var newGenre = new Genre
        {
            Id = userData.Id,
            Name = userData.Name ?? string.Empty
        };
        _dbContext.Add(newGenre);
        await _dbContext.SaveChangesAsync();
        var output = (GenreSaveDto)newGenre;
        return Ok(output);
    }

    // //PUT: api/Genres/5
    [HttpPut("{id}")]
    public async Task<ActionResult<GenreSaveDto>> PutGenre(int id, GenreSaveDto userData)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        var checkGenreName = _dbContext.Genres.Any(g => g.Name.Equals(userData.Name));
        if (genre != null && genre.Id != userData.Id) return NotFound(OperationResult.NOK("id diverso"));
        if (genre != null)
        {
            genre.Id = userData.Id;

            if (!checkGenreName)
                genre.Name = userData.Name ?? string.Empty;
        }

        await _dbContext.SaveChangesAsync();


        var newGenre = new GenreSaveDto
        {
            Id = userData.Id,
            Name = userData.Name
        };

        return Ok(newGenre);
    }

    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null) return NotFound();

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();
        return Ok(OperationResult.OK());
    }
}