using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Models.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FilmContext _dbcontext;

    public UnitOfWork(FilmContext context)
    {
        _dbcontext = context;
    }

    public async Task CompleteAsync()
    {
        await _dbcontext.SaveChangesAsync();
    }
}