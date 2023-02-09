using WebApplicationrRider.Domain.Models.Entity;

namespace WebApplicationrRider.Domain.Repositories;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetListAsync();
    Task<Actor?> Get(int id);
    Task<bool> ExistsAsync(string name, string surname);
    Task AddAsync(Actor actor);
    Task UpdateAsync(Actor actor);
    Task DeleteAsync(Actor id);
    IEnumerable<Film>  GetFilmByTitleAsync(IEnumerable<string?[]> title);
}