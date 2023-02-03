namespace WebApplicationrRider.Models;

public class FilmReferenceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public string? GenreName { get; set; } 
    
    public int TotalEarning { get; set; }
    public DateTime ReleaseDate { get; set; }
    
}