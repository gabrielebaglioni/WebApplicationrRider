using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Domain.Repositories;
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

        if (film.ActorsFilm.Any())
            await UpdateActorsAsync(existingFilm, film.ActorsFilm.Select(af => af.Actor));

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

    public Task<IEnumerable<Actor>> GetActorsByNameAndSurnameAsync(IEnumerable<Actor?> actorDtos)
    {
        return Task.FromResult(_DbContext.Actor.AsEnumerable()
            .Where(e => actorDtos
                .Any(dto => dto != null && dto.Name == e.Name && dto.Surname == e.Surname)));
    }


    public async Task UpdateActorsAsync(Film film, IEnumerable<Actor?> actorDtos)
    {
        var actors = await GetActorsByNameAndSurnameAsync(actorDtos);
        if(actors.Count()==0) 
            film.ActorsFilm.Clear();
        var notFoundActors = actorDtos
            .Where(dto => !actors
                .Any(a => dto != null && a.Name == dto.Name && a.Surname == dto.Surname))
            .Select(dto => $"Nome: {dto!.Name}, Cognome: {dto.Surname}");
        if (notFoundActors.Any())
            throw new Exception(
                $"Attenzione: alcuni attori non sono stati trovati nel database: {string.Join(", ", notFoundActors)}");

        var existingFilm = await _DbContext.Films
            .Include(f => f.ActorsFilm)
            .FirstOrDefaultAsync(f => f.Id == film.Id);
        if (existingFilm == null) return;

        existingFilm.ActorsFilm.Clear();
        foreach (var actor in actors) existingFilm.AddActor(actor);
        await _DbContext.SaveChangesAsync();
    }
}