using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Persistence.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FilmContext _context;
    private IActorRepository _actorRepository;
    private IFilmRepository _filmRepository;
    private IGenreRepository _genreRepository;
    private IUserRepository _userRepository;

    public UnitOfWork(FilmContext context, IFilmRepository filmRepository, IActorRepository actorRepository,
        IGenreRepository genreRepository, IUserRepository userRepository)
    {
        _context = context;
        _filmRepository = filmRepository;
        _actorRepository = actorRepository;
        _genreRepository = genreRepository;
        _userRepository = userRepository;
    }

    public IFilmRepository FilmRepository => _filmRepository ??= new FilmRepository(_context);

    public IActorRepository ActorRepository => _actorRepository ??= new ActorRepository(_context);

    public IGenreRepository GenreRepository => _genreRepository ??= new GenreRepository(_context);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}