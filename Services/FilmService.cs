
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
        return films.Select(film => (FilmOutputDto)film);
    }

    public async Task<FilmOutputDto> Get(int id)
    {
        var film = await _filmRepository.Get(id);
        if (film == null)
            throw new CheckException("Il film non è stato trovato nel database.");
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

            var actors = _filmRepository.GetActorsByNameAndSurname(filmSaveDto.Actors.Select(a => new[] {a?.Name, a?.Surname})).ToList();
            var notFoundActors = filmSaveDto.Actors
                .Where(dto => !actors
                    .Any(a => a.Name == dto?.Name && a.Surname == dto.Surname))
                .Select(dto => $"Nome: {dto?.Name}, Cognome: {dto?.Surname}");
            if (notFoundActors.Any())
            {
                throw new ActorNameAndSurnameNotValidException(name: actors.Select(a => a.Name).ToString(),
                    surname: actors.Select(a => a.Surname).ToString()!);
            }
            
            var film = (Film)filmSaveDto;
            film.Genre = genre;
            film.FkGenre = genre.Id;
            foreach (var actor in actors)
                film.AddActor(actor);

            await _filmRepository.AddAsync(film);
            return (FilmOutputDto)film;
    }


    public async Task<FilmOutputDto> UpdateAsync(int id, FilmSaveDto filmSaveDto)
    {
        var film = await _filmRepository.Get(id);
        if (film == null)
            throw new CheckException(" Il film non è stato trovato nel database.");
        
        if (filmSaveDto.Title != film.Title && await _filmRepository.ExistsAsync(filmSaveDto.Title))
            throw new CheckException("il titolo del film esiste già nel database.");
        
        if (film.EarningSale != null && filmSaveDto.TotalEarning != film.EarningSale.TotalEarning && filmSaveDto.TotalEarning <= 0)
            throw new CheckException(" Il TotalEarning  del film non può essere minore o uguale a 0.");
        
        if (filmSaveDto.GenreName == null)
            throw new CheckException(" Il genere non può essere null.");
        var genre = await _filmRepository.GetGenreByNameAsync(filmSaveDto.GenreName);
        if (film.Genre != null && filmSaveDto.GenreName != film.Genre.Name && genre == null)
            throw new CheckException("Il genere non è stato trovato nel database.");
        
        if (CheckChangesInToActors(filmSaveDto, film))
            film.DeleteAllActors();
        var actors = _filmRepository.GetActorsByNameAndSurname(filmSaveDto.Actors.Select(a => new[] {a?.Name, a?.Surname})).ToList();
        if (!actors.Any())
            film.ActorsFilm.Clear();
        var notFoundActors = filmSaveDto.Actors
            .Where(dto => !actors
                .Any(a => a.Name == dto?.Name && a.Surname == dto.Surname))
            .Select(dto => $"Nome: {dto?.Name}, Cognome: {dto?.Surname}");
        if (notFoundActors.Any())
            throw new CheckException(
                $"Gli attori {string.Join(", ", notFoundActors)} non sono stati trovati nel database.");
        foreach (var actor in actors)
            film.AddActor(actor);
        
        film.Title = filmSaveDto.Title;
        film.Genre = genre;
        if (genre != null)
            film.FkGenre = genre.Id;
        
        if (film.EarningSale != null) film.EarningSale.TotalEarning = filmSaveDto.TotalEarning;
        await _filmRepository.UpdateAsync(film);
        return (FilmOutputDto)film;
        
    }

    private static bool CheckChangesInToActors(FilmSaveDto filmSaveDto, Film film)
    {
        return !filmSaveDto.Actors
                   .All(aDto => film.ActorsFilm
                       .Any(afe => afe.Actor != null && afe.Actor.Name == aDto?.Name &&
                                   afe.Actor.Surname == aDto.Surname))
               || !film.ActorsFilm
                   .All(afe => afe.Actor != null && filmSaveDto.Actors
                       .Any(aDto => aDto?.Name == afe.Actor.Name && aDto.Surname == afe.Actor.Surname));
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



           
           