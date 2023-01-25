namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class FilmForOutputDTO
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string GenreName { get; set; }
    
    public DateTime ReleaseDate { get; set; }
}