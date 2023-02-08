using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;

namespace WebApplicationrRider.Services;

public class FilmService : IFilmServices
{
    private readonly IFilmRepository _filmRepository;

    public FilmService(IFilmRepository filmRepository)
    {
        _filmRepository = filmRepository;
    }

    public async Task<IEnumerable<FilmOutputDto>> GetListAsync()
    {
        var films = await _filmRepository.GetListAsync();
        var filmsOutput = films.Select(film => (FilmOutputDto)film).ToList();
        return filmsOutput;
    }

    public async Task<FilmOutputDto> Get(int id)
    {
        var film = await _filmRepository.Get(id);
        if (film == null)
            throw new FilmTitleNotValidException(title: film!.Title);
        var output = (FilmOutputDto)film;
        return output;
    }

    public async Task<FilmOutputDto> CreateAsync(FilmSaveDto filmSaveDto)
    {
        
            if (await _filmRepository.ExistsAsync(filmSaveDto.Title))
                throw new CheckException("Il film esiste già nel database.");

            var genre = await _filmRepository.GetGenreByNameAsync(filmSaveDto.GenreName ?? throw new InvalidOperationException(
                                                                      "Genre non può essere null"));
            if (genre == null)
                throw new CheckException("Il genere non è stato trovato nel database.");
            
            if (filmSaveDto.TotalEarning <= 0)
                throw new CheckException(" Il TotalEarning  del film non può essere minore o uguale a 0.");

            var actors = await _filmRepository.GetActorsByNameAndSurnameAsync(filmSaveDto.Actors.Select(a => a));
            var notFoundActors = filmSaveDto.Actors
                .Where(dto => !actors
                    .Any(a => a.Name == dto?.Name && a.Surname == dto.Surname))
                .Select(dto => $"Nome: {dto?.Name}, Cognome: {dto?.Surname}");
            if (notFoundActors.Any())
            {
                throw new ActorNameAndSurnameNotValidException(name: actors.Select(a => a.Name).ToString()!,
                    surname: actors.Select(a => a.Surname).ToString()!);
            }
            

            var film = (Film)filmSaveDto;
            film.Genre = genre;
            film.FkGenre = genre.Id;
            foreach (var actor in actors)
                film.AddActor(actor);

            await _filmRepository.AddAsync(film);

            var output = (FilmOutputDto)film;
            return output;
    }


    public async Task<FilmOutputDto> UpdateAsync(int id, FilmSaveDto filmSaveDto)
    {
        var existingFilm = await _filmRepository.Get(id);
            if (existingFilm == null)
                throw new CheckException(" Il film non è stato trovato nel database.");

            if (!await _filmRepository.ExistsAsync(filmSaveDto.Title) || existingFilm.Title == filmSaveDto.Title)
            {
                var genre = await _filmRepository.GetGenreByNameAsync(filmSaveDto.GenreName);
                if (genre == null)
                    throw new CheckException("Il genere non è stato trovato nel database.");
                if (filmSaveDto.TotalEarning <= 0)
                    throw new CheckException(" Il TotalEarning  del film non può essere minore o uguale a 0.");

                var actors = await _filmRepository.GetActorsByNameAndSurnameAsync(filmSaveDto.Actors);
                //se il count dei fulm passati è 0 allora elimino tutte le associazioni
                if (actors.Count() == 0)
                {
                    existingFilm.ActorsFilm.Clear();
                }
                else
                {
                    //se il count dei fulm passati è diverso dal count dei film nel db allora elimino le associazioni
                    if (actors.Count() != filmSaveDto.Actors.Count())
                    {
                        var notFoundActors = filmSaveDto.Actors
                            .Where(dto => !actors
                                .Any(a => a.Name == dto?.Name && a.Surname == dto.Surname))
                            .Select(dto => $"Nome: {dto?.Name}, Cognome: {dto?.Surname}");
                        if (notFoundActors.Any())
                            throw new CheckException(
                                $"Gli attori {string.Join(", ", notFoundActors)} non sono stati trovati nel database.");
                    }
                }

                var film = (Film)filmSaveDto;
                film.Genre = genre;
                film.FkGenre = genre.Id;
                film.ActorsFilm.Clear();
                foreach (var actor in actors) film.AddActor(actor);

                await _filmRepository.UpdateAsync(film);

                var output = (FilmOutputDto)film;
                return output;
            }

            return null!;
    }


    public async Task<FilmOutputDto> DeleteAsync(int id)
    {
        var film = await _filmRepository.Get(id);
        if (film == null)
            throw new CheckException(" Il film non è stato trovato nel database.");
        await _filmRepository.DeleteAsync(film);
        var output = (FilmOutputDto)film;
        return output;
    }
}