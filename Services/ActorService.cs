using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Models.Entity;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;

namespace WebApplicationrRider.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    public ActorService(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }
    public async Task<IEnumerable<ActorOutputDto>> GetListAsync()
    {
        var actors = await _actorRepository.GetListAsync();
        return actors.Select(actor => (ActorOutputDto)actor);
    }
    public async Task<ActorOutputDto> Get(int id)
    {
        var actor = await _actorRepository.Get(id);
        if (actor == null)
            throw new CheckException("l'attore non è stato trovato nel database.");
        return (ActorOutputDto)actor;
    }
    
    public async Task<ActorOutputDto> CreateAsync(ActorSaveDto actorSaveDto)
    {
        if(await _actorRepository.ExistsAsync(actorSaveDto.Name, actorSaveDto.Surname))
            throw new CheckException("L'attore esiste già nel database.");
        var films = _actorRepository.GetFilmByTitleAsync(actorSaveDto.Films.Select(f => new[] {f?.Title})).ToList();
        var notFoundFilm = actorSaveDto.Films
            .Where(dto => films.All(f => f.Title != dto?.Title))
            .Select(dto => $"Title: {dto.Title}");
        if(notFoundFilm.Any())
            throw new FilmTitleNotValidException(title: notFoundFilm.ToString());
        var actor = (Actor)actorSaveDto;
        foreach (var film in films)
            actor.AddFilm(film);
        
        await _actorRepository.AddAsync(actor);
        return (ActorOutputDto)actor;

    }
    
    public async Task<ActorOutputDto> UpdateAsync(int id, ActorSaveDto actorSaveDto)
    {
        var actor = await _actorRepository.Get(id);
        if (actor == null)
            throw new CheckException("L'attore non è stato trovato nel database.");
        if (actorSaveDto.Name != actor.Name || actorSaveDto.Surname != actor.Surname)
            if (await _actorRepository.ExistsAsync(actorSaveDto.Name, actorSaveDto.Surname))
                throw new CheckException("L'attore esiste già nel database.");
        if (CheckChangesInToFilms(actorSaveDto, actor))
            actor.DeleteAllFilms();
    {
            var films = _actorRepository.GetFilmByTitleAsync(actorSaveDto.Films.Select(f => new[] {f?.Title})).ToList();
            var notFoundFilm = actorSaveDto.Films
                .Where(dto => films.All(f => f.Title != dto?.Title))
                .Select(dto => $"Title: {dto.Title}");
            if(notFoundFilm.Any())
                throw new FilmTitleNotValidException(title: notFoundFilm.ToString());
            actor.FilmsActor.Clear();
            foreach (var film in films)
                actor.AddFilm(film);
    }

        await _actorRepository.UpdateAsync(actor);
        return (ActorOutputDto)actor;
    }

    public async Task<ActorOutputDto> DeleteAsync(int id)
    {
        var actor = await _actorRepository.Get(id);
        if (actor == null)
            throw new CheckException("L'attore non è stato trovato nel database.");
        await _actorRepository.DeleteAsync(actor);
        return (ActorOutputDto)actor;
    }

    private static bool CheckChangesInToFilms(ActorSaveDto actorSaveDto, Actor actor)
    {
        return !actorSaveDto.Films
                   .All(fDto => actor.FilmsActor
                       .Any(f => f.Film != null && f.Film.Title == fDto.Title))
               || !actor.FilmsActor
                   .All(f => actorSaveDto.Films
                       .Any(fDto => f.Film != null && f.Film.Title == fDto.Title));
    }
}