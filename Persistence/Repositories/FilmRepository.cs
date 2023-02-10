using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Entity;
using WebApplicationrRider.Models;
using WebApplicationrRider.Models.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class FilmRepository : BaseRepository, IFilmRepository
{
    public FilmRepository(FilmContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Film>> GetListAsync()
    {
        return await _DbContext.Films
            .Include(f => f.Genre)
            .Include(f => f.EarningSale)
            .Include(f => f.ActorsFilm)
            .ThenInclude(af => af.Actor)
            .ToListAsync();
    }

    public Task<Film?> Get(int id)
    {
        return _DbContext.Films
            .Include(f => f.Genre)
            .Include(f => f.EarningSale)
            .Include(f => f.ActorsFilm)
            .ThenInclude(af => af.Actor)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<bool> ExistsAsync(string title)
    {
        return _DbContext.Films.AnyAsync(x => x.Title == title);
    }

    public async Task AddAsync(Film film)
    {
        await _DbContext.Films.AddAsync(film);
        await _DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Film film)
    {
        var existingFilm = await _DbContext.Films
            .Include(f => f.Genre)
            .Include(f => f.EarningSale)
            .Include(f => f.ActorsFilm)
            .ThenInclude(af => af.Actor)
            .FirstOrDefaultAsync(f => f.Id == film.Id);
        if (existingFilm == null) return;

        _DbContext.Entry(existingFilm).CurrentValues.SetValues(film);
        await _DbContext.SaveChangesAsync();
        
    }


    public async Task DeleteAsync(Film film)
    {
        _DbContext.Films.Remove(film);
        await _DbContext.SaveChangesAsync();
    }

    public Task<Genre?> GetGenreByNameAsync(string? name)
    {
        return _DbContext.Genres.FirstOrDefaultAsync(g => g.Name == name);
    }

    public IEnumerable<Actor> GetActorsByNameAndSurname(IEnumerable<string?[]> nameSurnameList)
    {
        return _DbContext.Actor.AsEnumerable()
            .Where(e => nameSurnameList
                .Any(a => a[0] == e.Name && a[1] == e.Surname));
    }
    
}


