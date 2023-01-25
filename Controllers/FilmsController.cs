using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly FilmContext _dbContext;
        private readonly IMapper _mapper;
        private static List<Film> films = new List<Film>();
        public FilmsController(FilmContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        // GET: api/Films
        [HttpGet]
        public  ActionResult GetAllFilms()
        {
            if (!_dbContext.Films.Any())
                return NotFound();
            
            var allFilms = _dbContext.Films.ToList();
            var _films = _mapper.Map<IEnumerable<FilmForOutputDTO>>(allFilms);
            return  Ok(_films);
        }
        //GET: api/Films/2
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmForCreationDTO>> GetMovieById(int id)
        {
            var film = (FilmForCreationDTO?)await _dbContext.Films
                //.Include(x =>x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (film == null)
            {
                return NotFound();
            }
            
            return film;
        }
        //POST: api/Films
        [HttpPost]
        
        public async Task<ActionResult<FilmForCreationDTO>> PostFilm(/*[FromBody]*/ FilmForCreationDTO data)
        {
            //var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Id.Equals(film.GenreId));
            //var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));
            var check = _dbContext.Genres.Any(genres => genres.Name.Equals(data.GenreName));
        
            // if (/*!checkFilmGenreExist ||*/ !checkFilmTitleExist)
            // {
            //     return BadRequest(OperationResult.NOK("Genere Inesistente o Titolo del film gia in uso"));
            // }
            if (/*!checkFilmGenreExist ||*/ !check)
            {
                return BadRequest(OperationResult.NOK("Genere Inesistente"));
            }

            var film = _mapper.Map<Film>(data);
            _dbContext.Films.Add(film);
            var newFilm = _mapper.Map<FilmForOutputDTO>(film);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllFilms), new { id = film.Id }, newFilm);
        }
        
        // //PUT:api/Films/3
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
           // var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Id.Equals(film.GenreId));
            var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));
            if (id != film.Id )
            {
                return NotFound(OperationResult.NOK("id diverso"));
            }
            _dbContext.Entry(film).State = EntityState.Modified;
            try
            {
                if(/*checkFilmGenreExist ||*/ checkFilmTitleExist)
                {
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(OperationResult.NOK("Il genre non esiste"));
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExist(id) )
                {
                    return NotFound(OperationResult.NOK("Id inesistente"));
                }
                else
                {
                    throw;
                }
            }
        
            return NoContent();
        }
        // //DELETE: api/Films/2
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteFilm(int id)
        // {
        //     if (_dbContext == null)
        //     {
        //         return NotFound(OperationResult.NOK());
        //     }
        //
        //     var film = await _dbContext.Films.FindAsync(id);
        //     if (film == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _dbContext.Films.Remove(film);
        //     await _dbContext.SaveChangesAsync();
        //     return NoContent();
        // }
        private bool FilmExist(long id)
        {
            return (_dbContext.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

