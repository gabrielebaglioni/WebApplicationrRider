using WebApplicationrRider.Domain.Models.DTOs.References;
using WebApplicationrRider.Domain.Models.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Outgoing;

public class ActorOutputDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public ICollection<FilmReferenceDto> Films { get; set; } = new List<FilmReferenceDto>();

    public static explicit operator ActorOutputDto(Actor actor)
    {
        var dto = new ActorOutputDto();
        dto.Name = actor.Name;
        dto.Surname = actor.Surname;
        dto.BirthDate = actor.Birthdate;
        foreach (var actorFilm in actor.FilmsActor)
        {
            if (actorFilm.Film?.Title == null) continue;
            var filmReferenceDto = new FilmReferenceDto
            {
                Id = actorFilm.Film.Id,
                Title = actorFilm.Film.Title,
                ReleaseDate = actorFilm.Film.ReleaseDate,
                GenreName = actorFilm.Film.Genre?.Name ?? string.Empty,
                TotalEarning = actorFilm.Film.EarningSale?.TotalEarning ?? 0
            };
            dto.Films.Add(filmReferenceDto);
        }

        return dto;
    }
}