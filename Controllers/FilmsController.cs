using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;

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
        public async Task<ActionResult<IEnumerable<Film>>> GetAllFilms()
        {
            if (_dbContext.Films == null)
            {
                return NotFound();
            }

            return await _dbContext.Films.ToListAsync();
        }
        //GET: api/Films/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetMovieById(int id)
        {
            if (_dbContext.Films == null)
            {
                return NotFound();
            }
            var film = await _dbContext.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            return film;
        }
        //POST: api/Films
        [HttpPost]

        public async Task<ActionResult<Film>> PostFilm(/*[FromBody]*/ Film film)
        {
            var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Name.Equals(film.GenreName));
            var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));

            if (!checkFilmGenreExist || !checkFilmTitleExist)
            {
                return BadRequest(OperationResult.NOK("Genere Inesistente o Titolo del film gia in uso"));
            }
            //inserire altri controlli
            _dbContext.Films.Add(film);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllFilms), new { id = film.Id }, film);
        }
        
        //PUT:api/Films/3
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Name.Equals(film.GenreName));
            var checkFilmTitleExist = _dbContext.Films.Any(film => film.Title.Equals(film.Title));
            if (id != film.Id )
            {
                return NotFound(OperationResult.NOK("id diverso"));
            }
            _dbContext.Entry(film).State = EntityState.Modified;
            try
            {
                if(checkFilmGenreExist || checkFilmTitleExist)
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
        //DELETE: api/Films/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var film = await _dbContext.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            _dbContext.Films.Remove(film);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        private bool FilmExist(long id)
        {
            return (_dbContext.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

