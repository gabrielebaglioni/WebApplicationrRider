using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;
using GenreOutputDTO = WebApplicationrRider.Models.GenreOutputDTO;

namespace WebApplicationrRider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private static List<Genre> _genres = new();
    private readonly FilmContext _dbContext;
    

    public GenresController(FilmContext dbContext)
    {
        _dbContext = dbContext;
    }

    //GET: api/Genres
    [HttpGet]
    public ActionResult<IEnumerable<Models.DTOs.Outgoing.GenreOutputDTO>> GetAllGenres()
    {
        if (_dbContext.Genres == null) return NotFound();
        var allGenre = _dbContext.Films.Include("Genre").ToList();
        //var _filmsDtos = _mapper.Map<IEnumerable<FilmForOutputDTO>>(allFilms);
        var _GenreDtos = allGenre.Select(f => (FilmForOutputDTO)f);
        return Ok(_GenreDtos);
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
    public ActionResult<Models.DTOs.Outgoing.GenreOutputDTO> GetGenreById(int id)
    {
        var genre = _dbContext.Genres
            //.Include(x =>x.Genre)
            .FirstOrDefault(x => x.Id == id);
        if (genre == null)
            return NotFound();

        var _genre = (GenreOutputDTO)genre;
        return Ok(_genre);
    }

    //POST: api/Genres
    [HttpPost]
    public async Task<ActionResult<List<FilmForOutputDTO>>> PostGenre(GenreOutputDTO data)
    {
        var check = _dbContext.Genres.Any(genres => genres.Name.Equals(data.Name));
        if ( /*!checkFilmGenreExist ||*/ check) return BadRequest(OperationResult.NOK("Genere già Inesistente"));
       // var genre = _mapper.Map<Genre>(data);
       // _dbContext.Genres.Add(GenreOutputDTO(data));
        //var newGenre = _mapper.Map<Models.DTOs.Outgoing.GenreOutputDTO>(genre);
        await _dbContext.SaveChangesAsync();
        return NoContent();/*CreatedAtAction(nameof(GetAllGenres), new { id = genre.Id }, newGenre)*/;
    }

    // //PUT: api/Genres/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Models.DTOs.Outgoing.GenreOutputDTO>> PutGenre(int id, GenreOutputDTO data)
    {
        var checkGenreNameExist = _dbContext.Genres.Any(genre => genre.Name.Equals(data.Name));
       // var _genre = _mapper.Map<Genre>(data);
        if (id != data.Id) return NotFound(OperationResult.NOK("id diverso"));
       // _dbContext.Entry(_genre).State = EntityState.Modified;
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