using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.References;

public class ActorReferenceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public static explicit operator ActorReferenceDto(Actor actor)
    {
        var dto = new ActorReferenceDto();
        dto.Id = actor.Id;
        dto.Name = actor.Name;
        dto.Surname = actor.Surname;
        dto.BirthDate = actor.Birthdate;
        return dto;
    }
}