namespace WebApplicationrRider.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IFilmRepository FilmRepository { get; }
    IActorRepository ActorRepository { get; }
    IGenreRepository GenreRepository { get; }
    IUserRepository UserRepository { get; }

    Task<int> SaveChangesAsync();
}