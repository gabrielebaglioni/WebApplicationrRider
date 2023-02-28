using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetListAsync();
    Task<Genre?> Get(int id);
    Task<bool> ExistsAsync(string name);
    Task AddAsync(Genre genre);

    Task UpdateAsync(Genre genre);

    Task DeleteAsync(Genre genre);
}