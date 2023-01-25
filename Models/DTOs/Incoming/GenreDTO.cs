namespace WebApplicationrRider.Models;

public class GenreDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public static explicit operator  GenreDTO(Genre entity)
    {
        return new GenreDTO()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}