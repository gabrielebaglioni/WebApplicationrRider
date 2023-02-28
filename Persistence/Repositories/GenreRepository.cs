using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Entity;
using WebApplicationrRider.Persistence.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class GenreRepository : BaseRepository, IGenreRepository
{
    public GenreRepository(FilmContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Genre>> GetListAsync()
    {
        return await _DbContext.Genres.ToListAsync();
    }

    public Task<Genre?> Get(int id)
    {
        return _DbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Genre genre)
    {
        await _DbContext.Genres.AddAsync(genre);
        await _DbContext.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(string name)
    {
        return _DbContext.Genres.AnyAsync(x => x.Name == name);
    }

    public Task UpdateAsync(Genre genre)
    {
        return Task.FromResult(_DbContext.Genres.Update(genre));
    }

    public async Task DeleteAsync(Genre genre)
    {
        _DbContext.Genres.Remove(genre);
        await _DbContext.SaveChangesAsync();
    }
}