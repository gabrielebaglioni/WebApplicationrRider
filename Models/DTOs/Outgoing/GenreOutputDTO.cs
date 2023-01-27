namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class GenreOutputDTO
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public  List<Film> Films { get; set; }
    
    public static explicit operator GenreOutputDTO(Genre entity)
    {
        return new GenreOutputDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
}