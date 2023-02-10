using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Outgoing;

public class GenreOutputDto
{
    public int Id { get; set; }

    public string? Name { get; set; }


    public static explicit operator GenreOutputDto(Genre entity)
    {
        return new GenreOutputDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}