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
    private static List<Film> films = new();
    private readonly FilmContext _dbContext;
    private readonly IMapper _mapper;

    public FilmsController(FilmContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // GET: api/Films
    [HttpGet]
    public ActionResult<IEnumerable<FilmForOutputDTO>> GetAllFilms()
    {
        if (!_dbContext.Films.Any())
            return NotFound();

        var allFilms = _dbContext.Films.ToList();
        var _films = _mapper.Map<IEnumerable<FilmForOutputDTO>>(allFilms);
        // var _films = allFilms.Select(f => (FilmForOutputDTO)f);
        return Ok(_films);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public ActionResult<FilmForOutputDTO> GetMovieById(int id)
    {
        var film = _dbContext.Films
            //.Include(x =>x.Genre)
            .FirstOrDefault(x => x.Id == id);
        if (film == null)
            return NotFound();

        var _film = _mapper.Map<FilmForOutputDTO>(film);


        return Ok(_film);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<FilmForOutputDTO>> PostFilm( /*[FromBody]*/ FilmSaveDTO data)
    {
        //var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Id.Equals(film.GenreId));
        //var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));
        var check = _dbContext.Genres.Any(genres => genres.Name.Equals(data.GenreName));

        // if (/*!checkFilmGenreExist ||*/ !checkFilmTitleExist)
        // {
        //     return BadRequest(OperationResult.NOK("Genere Inesistente o Titolo del film gia in uso"));
        // }
        if ( /*!checkFilmGenreExist ||*/ !check) return BadRequest(OperationResult.NOK("Genere Inesistente"));

        var film = _mapper.Map<Film>(data);
        _dbContext.Films.Add(film);
        var newFilm = _mapper.Map<FilmForOutputDTO>(film);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllFilms), new { id = film.Id }, newFilm);
    }

    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<ActionResult<FilmForOutputDTO>> PutFilm(int id, FilmSaveDTO data)
    {
        // var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Id.Equals(film.GenreId));
        var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));

        var _film = _mapper.Map<Film>(data);
        //var UpdatedFilm = _mapper.Map<FilmForOutputDTO>(_film);

        if (id != data.Id) return NotFound(OperationResult.NOK("id diverso"));
        _dbContext.Entry(_film).State = EntityState.Modified;
        try
        {
            if ( /*checkFilmGenreExist ||*/ checkFilmTitleExist)
                await _dbContext.SaveChangesAsync();
            else
                return BadRequest(OperationResult.NOK("Il genre non esiste"));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FilmExist(id))
                return NotFound(OperationResult.NOK("Id inesistente"));
            throw;
        }

        return NoContent();
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

    private bool FilmExist(long id)
    {
        return (_dbContext.Films?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}