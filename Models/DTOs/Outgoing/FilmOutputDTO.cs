using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class FilmOutputDto
{
    public int Id { get;set; }

    public string? Title { get; set; }

    public string? GenreName { get; set; }

    public int TotalEarning { get; set; }

    public DateTime ReleaseDate { get; set; }

    public ICollection<ActorOutputDto> Actors { get; set; } = new List<ActorOutputDto>();
    public static explicit operator FilmOutputDto(Film entity)
    {
        var dto = new FilmOutputDto();
            dto.Id = entity.Id;
            dto.Title = entity.Title;
            dto.GenreName = entity.Genre?.Name ?? string.Empty;
            if (entity.EarningSale != null) dto.TotalEarning = entity.EarningSale.TotalEarning;
            dto.ReleaseDate = entity.ReleaseDate;
            
            foreach (var actorFilm in entity.ActorsFilm)
            {
                if (actorFilm.Actor == null) continue;
                    var actorDto = (ActorOutputDto)actorFilm.Actor;
                    dto.Actors.Add(actorDto);
            }
            return dto;
        }
}