using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models;

namespace WebApplicationrRider.Controllers
{
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

        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenres()
        {
            if (_dbContext.Genres == null)
            {
                return NotFound();
            }
            return await _dbContext.Genres.ToListAsync();
        }

        //GET: api/Genres/5
        // [HttpGet("{GenreId}")]
        //
        // public async Task<ActionResult<List<Film>>> GetFilmByGenre(int GenreId)
        // {
        //     if (_dbContext.Genres == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var filmsByGenre = await _dbContext.Films
        //         .Where(x => x.GenreId == GenreId)
        //         .ToListAsync();
        //
        //     return filmsByGenre;
        // }

        //POST: api/Genres
         [HttpPost]
         public async Task<ActionResult<List<Genre>>> PostGenre(CreateGenreDto request)
         {
             var newGenre = new Genre
             {
                 Name = request.Name
             };
             _dbContext.Genres.Add(newGenre);
             await _dbContext.SaveChangesAsync();
        
             return CreatedAtAction(nameof(GetAllGenres), new { id = newGenre.Id }, newGenre);
         }
        
        // //PUT: api/Genres/5
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutGenre(int id, CreateGenreDto request)
        {
           
           
            var checkGenreNameExist = _dbContext.Genres.Any(genre => genre.Name.Equals(genre.Name));
            var newGenre = new Genre
            {
                Name = request.Name
            };
           
            _dbContext.Entry(request).State = EntityState.Modified;
            
            try
            {
                if(checkGenreNameExist)
                {
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(OperationResult.NOK("Il genre già esiste"));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteGenre(int id)
        // {
        //     if (_dbContext == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var genre = await _dbContext.Genres.FindAsync(id);
        //     if (genre == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _dbContext.Genres.Remove(genre);
        //     await _dbContext.SaveChangesAsync();
        //     return NoContent();
        // }
        // private bool FilmExist(long id)
        // {
        //     return (_dbContext.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        // }

        private bool GenreExists(long id)
        {
            return (_dbContext.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
