using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private static List<Genre> _genres = new();
    private readonly FilmContext _dbContext;
    private readonly IMapper _mapper;

    public GenresController(FilmContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    //GET: api/Genres
    [HttpGet]
    public ActionResult<IEnumerable<GenreOutputDTO>> GetAllGenres()
    {
        if (_dbContext.Genres == null) return NotFound();
        var allGenres = _dbContext.Genres.ToList();
        var _genres = _mapper.Map<IEnumerable<GenreOutputDTO>>(allGenres);
        return Ok(_genres);
    }

    // GET: api/Genres/5
    // GET ALL FILMS BY GENRE
    // [HttpGet("{GenreId}")]
    // public async Task<ActionResult<List<Film>>> GetFilmByGenre(int GenreId)
    // {
    //     if (_dbContext.Genres == null) return NotFound();
    //
    //     var filmsByGenre = await _dbContext.Films
    //         //.Where(x => x.GenreId == GenreId)
    //         .ToListAsync();
    //
    //     return filmsByGenre;
    // }

    //GET: api/Genres/5
    //GET A SINGLE GENRE BY ID
    [HttpGet("{id}")]
    public ActionResult<GenreOutputDTO> GetGenreById(int id)
    {
        var genre = _dbContext.Genres
            //.Include(x =>x.Genre)
            .FirstOrDefault(x => x.Id == id);
        if (genre == null)
            return NotFound();

        var _genre = _mapper.Map<GenreOutputDTO>(genre);
        return Ok(_genre);
    }

    //POST: api/Genres
    [HttpPost]
    public async Task<ActionResult<List<FilmForOutputDTO>>> PostGenre(GenreSaveDTO data)
    {
        var check = _dbContext.Genres.Any(genres => genres.Name.Equals(data.Name));
        if ( /*!checkFilmGenreExist ||*/ check) return BadRequest(OperationResult.NOK("Genere già Inesistente"));
        var genre = _mapper.Map<Genre>(data);
        _dbContext.Genres.Add(genre);
        var newGenre = _mapper.Map<GenreOutputDTO>(genre);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllGenres), new { id = genre.Id }, newGenre);
    }

    // //PUT: api/Genres/5
    [HttpPut("{id}")]
    public async Task<ActionResult<GenreOutputDTO>> PutGenre(int id, GenreSaveDTO data)
    {
        var checkGenreNameExist = _dbContext.Genres.Any(genre => genre.Name.Equals(data.Name));
        var _genre = _mapper.Map<Genre>(data);
        if (id != data.Id) return NotFound(OperationResult.NOK("id diverso"));
        _dbContext.Entry(_genre).State = EntityState.Modified;
        try
        {
            if (!checkGenreNameExist)
                await _dbContext.SaveChangesAsync();
            else
                return BadRequest(OperationResult.NOK("Il genre già esiste"));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenreExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        if (_dbContext == null) return NotFound(OperationResult.NOK());

        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null) return NotFound();

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }


    private bool GenreExists(long id)
    {
        return (_dbContext.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}