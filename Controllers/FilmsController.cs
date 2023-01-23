using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            var Film = await _dbContext.Films.FindAsync(id);

            if (Film == null)
            {
                return NotFound();
            }

            return Film;
        }
        //POST: api/Films
        [HttpPost]

        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            _dbContext.Films.Add(film);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovieById), new { id = film.Id }, film);
        }
        //PUT:api/Films/3
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            if (id != film.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(film).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExist(id))
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

