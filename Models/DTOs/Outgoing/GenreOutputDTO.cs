using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class GenreOutputDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public  List<Film>? Films { get; set; }
    
    public static explicit operator GenreOutputDto(Genre entity)
    {
        return new GenreOutputDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
}