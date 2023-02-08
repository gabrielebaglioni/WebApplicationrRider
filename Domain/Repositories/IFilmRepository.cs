using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Models;

namespace WebApplicationrRider.Domain.Repositories;

public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetListAsync();
    Task<Film?> Get(int id);
    Task<bool> ExistsAsync(string Title);
    Task AddAsync(Film film);
    Task UpdateAsync(Film film);
    Task DeleteAsync(Film film);
    Task<IEnumerable<Actor>> GetActorsByNameAndSurnameAsync(IEnumerable<Actor?> actorDtos);
    Task<Genre?> GetGenreByNameAsync(string? name);
    Task UpdateActorsAsync(Film existingFilm, IEnumerable<Actor?> actors);
}