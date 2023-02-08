using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.Models;

namespace WebApplicationrRider.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;


    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreOutputDto>> GetListAsync()
    {
        var genres = await _genreRepository.GetListAsync();
        var genresOutput = genres.Select(genre => (GenreOutputDto)genre).ToList();
        return genresOutput;
    }

    public async Task<GenreOutputDto> Get(int id)
    {
        var genre = await _genreRepository.Get(id);
        if (genre == null)
            throw new CheckException("Il genere non esiste nel database.");
        var ouput = (GenreOutputDto)genre;
        return ouput;
    }

    public async Task<GenreOutputDto> CreateAsync(GenreSaveDto genreSaveDto)
    {
        if (await _genreRepository.ExistsAsync(genreSaveDto.Name))
            throw new CheckException("Il genere esiste già nel database.");
        var genre = (Genre)genreSaveDto;
        await _genreRepository.AddAsync(genre);
        var output = (GenreOutputDto)genre;
        return output;
    }

    public async Task<GenreOutputDto> UpdateAsync(int id, GenreSaveDto genreSaveDto)
    {
        if (await _genreRepository.ExistsAsync(genreSaveDto.Name))
                throw new CheckException("Il genere esiste già nel database.");

        var genre = (Genre)genreSaveDto;
        genre.Id = id;
        await _genreRepository.UpdateAsync(genre); 
        var output = (GenreOutputDto)genre;
        return output;
    }
    public async Task<GenreOutputDto> DeleteAsync(int id)
    {
        var genre = await _genreRepository.Get(id);
        if (genre == null)
            throw new CheckException("Il genere non esiste nel database.");
        await _genreRepository.DeleteAsync(genre);
        return (GenreOutputDto)genre;
    }
}