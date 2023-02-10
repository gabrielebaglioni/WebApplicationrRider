﻿using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Services;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<IEnumerable<User>> GetAll();
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new()
    {
        new() { Id = 1, FirstName = "Gabriele", LastName = "Baglioni", Username = "test", Password = "test" }
    };

    public async Task<User> Authenticate(string username, string password)
    {
        // wrapped in "await Task.Run" to mimic fetching user from a db
        var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

        // on auth fail: null is returned because user is not found
        // on auth success: user object is returned
        return user ?? throw new CheckException("login failed");
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        // wrapped in "await Task.Run" to mimic fetching users from a db
        return await Task.Run(() => _users);
    }
}