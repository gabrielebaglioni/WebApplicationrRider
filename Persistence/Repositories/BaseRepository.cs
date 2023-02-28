using WebApplicationrRider.Persistence.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly FilmContext _DbContext;

    public BaseRepository(FilmContext dbContext)
    {
        _DbContext = dbContext;
    }
}