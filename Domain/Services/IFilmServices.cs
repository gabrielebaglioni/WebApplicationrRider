using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Services;

public interface IFilmServices
{
    Task<IEnumerable<FilmOutputDto>> GetListAsync();
    Task<FilmOutputDto> Get(int id);
    Task<FilmOutputDto> CreateAsync(FilmSaveDto filmSaveDto);
    Task<FilmOutputDto> UpdateAsync(int id, FilmSaveDto filmSaveDto);
    Task<FilmOutputDto> DeleteAsync(int id);
}