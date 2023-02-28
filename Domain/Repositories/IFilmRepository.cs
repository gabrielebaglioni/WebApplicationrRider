using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Repositories;

public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetListAsync();
    Task<Film?> Get(int id);
    Task AddAsync(Film film);
    Task UpdateAsync(Film film);
    Task DeleteAsync(Film film);
    Task<bool> ExistsAsync(string Title);
    IEnumerable<Actor> GetActorsByNameAndSurname(IEnumerable<string?[]> nameSurnameList);
    Task<Genre?> GetGenreByNameAsync(string? name);
}