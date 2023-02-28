using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Services;

public interface IActorService
{
    Task<IEnumerable<ActorOutputDto>> GetListAsync();
    Task<ActorOutputDto> Get(int id);
    Task<ActorOutputDto> CreateAsync(ActorSaveDto actorSaveDto);
    Task<ActorOutputDto> UpdateAsync(int id, ActorSaveDto actorSaveDto);
    Task<ActorOutputDto> DeleteAsync(int id);
}