using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Models;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.Entity;
using WebApplicationrRider.Helpers;

namespace WebApplicationrRider.Services;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }


    public async Task<IEnumerable<UserOutputDto>> GetListAsync()
    {
        var users = await _userRepository.GetListAsync();
        var usersOutput = users.Select(user => (UserOutputDto)user).ToList();
        return usersOutput;
    }


    public async Task<UserOutputDto> Get(int id)
    {
        var user = await _userRepository.Get(id);
        if (user == null)
            throw new CheckException("User not found");
        var output = (UserOutputDto)user;
        return output;
    }

    public async Task<UserOutputDto> CreateAsync(UserSaveDto userSaveDto)
    {
        if (await _userRepository.GetUserByUsername(userSaveDto.Username))
            throw new CheckException("Username \"" + userSaveDto.Username + "\" is already taken");
        //fai un cotrollo anche sulla mail
        var user = (User)userSaveDto;
        await _userRepository.AddAsync(user);
        var output = (UserOutputDto)user;
        return output;
    }

    public async Task<UserOutputDto> UpdateAsync(int id, UserSaveDto userSaveDto)
    {
        if (await _userRepository.GetUserByUsername(userSaveDto.Username))
            throw new CheckException("Username \"" + userSaveDto.Username + "\" is already taken");
        var user = (User)userSaveDto;
        user.Id = id;
        await _userRepository.UpdateAsync(user);
        var output = (UserOutputDto)user;
        return output;
    }

    public async Task<UserOutputDto> DeleteAsync(int id)
    {
        var user = await _userRepository.Get(id);
        if (user == null)
            throw new CheckException("User not found");
        await _userRepository.DeleteAsync(user);
        var output = (UserOutputDto)user;
        return output;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepository.GetUserByUsernameAndPassword(model.Username, model.Password);

        if (user != null)
        {
            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        return null;
    }


    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 30 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}