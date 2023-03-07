using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationrRider.Domain.Services;

public interface IAuthenticateService
{
    Task<JwtSecurityToken?> AuthenticateUser(string username, string password);
    Task<IdentityResult> Register(string username, string email, string password);
    Task<IdentityResult> RegisterAdmin(string username, string email, string password);
    Task<bool> CheckIfUsernameExistsAsync(string username);
    Task<bool> CheckIfEmailExistsAsync(string email);
}