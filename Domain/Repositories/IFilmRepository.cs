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
    IEnumerable<Actor> GetActorsByNameAndSurname(IEnumerable<string?[]> nameSurnameList);
    Task<Genre?> GetGenreByNameAsync(string? name);
   
}