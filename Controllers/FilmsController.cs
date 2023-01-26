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

        var allFilms = _dbContext.Films.Include("Genre").ToList();
        //var _filmsDtos = _mapper.Map<IEnumerable<FilmForOutputDTO>>(allFilms);
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

        var _filmsDtos = _mapper.Map<FilmForOutputDTO>(film);
       


        return Ok(_filmsDtos);
    }

    //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<FilmForOutputDTO>> PostFilm( /*[FromBody]*/ FilmSaveDTO data)
    {
        // var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Id.Equals(data.Id));
        // var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(data.Title));
        //var check = _dbContext.Genres.Any(genres => genres.Name.Equals(data.GenreName));

        // if (!checkFilmGenreExist || checkFilmTitleExist)
        // {
        //     return BadRequest(OperationResult.NOK("Genere Inesistente o Titolo del film gia in uso"));
        // }

        // var film = _mapper.Map<Film>(data);
        // _dbContext.Films.Add(film);
        // var film = _dbContext.Films.Include("Genre").FirstOrDefaultAsync();
        // var newFilm = _mapper.Map<FilmForOutputDTO>(film);
        // await _dbContext.SaveChangesAsync();
        // return CreatedAtAction(nameof(GetAllFilms), new { id = film.Id }, newFilm);
        var newFilm = _mapper.Map<Film>(data);
        // var newFilm = _dbContext.Films.Include("Genre");
        _dbContext.Films.Add(newFilm);
        await _dbContext.SaveChangesAsync();
        return Ok(new FilmForOutputDTO());
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