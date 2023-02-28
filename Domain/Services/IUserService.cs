using WebApplicationrRider.Domain.Models;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Services;

public interface IUserService
{
    Task<IEnumerable<UserOutputDto>> GetListAsync();
    Task<UserOutputDto> Get(int id);
    Task<UserOutputDto> CreateAsync(UserSaveDto userSaveDto);
    Task<UserOutputDto> UpdateAsync(int id, UserSaveDto userSaveDto);
    Task<UserOutputDto> DeleteAsync(int id);
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
}