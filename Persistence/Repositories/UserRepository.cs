using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Entity;
using WebApplicationrRider.Persistence.Context;

namespace WebApplicationrRider.Persistence.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(FilmContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<User>> GetListAsync()
    {
        return await _DbContext.Users.ToListAsync();
    }

    public async Task<User?> Get(int id)
    {
        var user = await _DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            throw new CheckException("User not found");
        return user;
    }

    public async Task AddAsync(User user)
    {
        await _DbContext.Users.AddAsync(user);
        await _DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _DbContext.Users.Update(user);
        await _DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _DbContext.Users.Remove(user);
        await _DbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsernameAndPassword(string modelUsername, string modelPassword)
    {
        return await _DbContext.Users.FirstOrDefaultAsync(x =>
            x.Username == modelUsername && x.Password == modelPassword);
    }

    //genera il metodo GetUserByUsername che ritorna bool
    public async Task<bool> GetUserByUsername(string username)
    {
        return await _DbContext.Users.AnyAsync(x => x.Username == username);
    }
}