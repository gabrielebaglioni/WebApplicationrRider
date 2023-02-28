using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetListAsync();
    Task<User?> Get(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> GetUserByUsernameAndPassword(string modelUsername, string modelPassword);
    Task<bool> GetUserByUsername(string username);
}