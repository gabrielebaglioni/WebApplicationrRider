using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;

namespace WebApplicationrRider.Domain.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreOutputDto>> GetListAsync();

    Task<GenreOutputDto> Get(int id);

    Task<GenreOutputDto> CreateAsync(GenreSaveDto genreSaveDto);

    Task<GenreOutputDto> UpdateAsync(int id, GenreSaveDto genreSaveDto);
    Task<GenreOutputDto> DeleteAsync(int id);
}