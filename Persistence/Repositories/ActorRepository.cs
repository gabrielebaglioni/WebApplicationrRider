using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Models.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class ActorRepository : BaseRepository, IActorRepository
{
    public ActorRepository(FilmContext dbContext) : base(dbContext)
    {
    }
    public async Task<IEnumerable<Actor>> GetListAsync()
    {
        return await _DbContext.Actor
            .Include(a => a.FilmsActor)
                .ThenInclude(fa => fa.Film)
                    .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
                .ThenInclude(fa => fa.Film)
                    .ThenInclude(film => film!.EarningSale)
            .ToListAsync();
    }
    
    public async Task<Actor?> Get(int id)
    {
        return await _DbContext.Actor
            .Include(a => a.FilmsActor)
                .ThenInclude(fa => fa.Film)
                    .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
                 .ThenInclude(fa => fa.Film)
                     .ThenInclude(film => film!.EarningSale)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<bool> ExistsAsync(string name, string surname)
    {
        return await _DbContext.Actor.AnyAsync(x => x.Name == name && x.Surname == surname);
    }
    
    public async Task AddAsync(Actor actor)
    {
        await _DbContext.Actor.AddAsync(actor);
        await _DbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Actor actor)
    {
        var existingActor = await _DbContext.Actor
            .Include(a => a.FilmsActor)
                .ThenInclude(fa => fa.Film)
                    .ThenInclude(film => film!.Genre)
            .Include(a => a.FilmsActor)
                .ThenInclude(fa => fa.Film)
                    .ThenInclude(film => film!.EarningSale)
            .FirstOrDefaultAsync(a => a.Id == actor.Id);
        if (existingActor == null) return;
        
        _DbContext.Entry(existingActor).CurrentValues.SetValues(actor);
        await _DbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Actor id)
    {
        var actor = await _DbContext.Actor.FindAsync(id);
        if (actor == null) return;
        
        _DbContext.Actor.Remove(actor);
        await _DbContext.SaveChangesAsync();
    }
    

    public  IEnumerable<Film>  GetFilmByTitleAsync(IEnumerable<string?[]> title)
    {
        return _DbContext.Films.AsEnumerable()
            .Where(f => title.Any(a => a[0] == f.Title));
    }

}