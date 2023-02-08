namespace WebApplicationrRider.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}