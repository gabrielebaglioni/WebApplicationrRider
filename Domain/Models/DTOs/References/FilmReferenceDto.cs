namespace WebApplicationrRider.Domain.Models.DTOs.References;

public class FilmReferenceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public string GenreName { get; set; } = null!;

    public int TotalEarning { get; set; }
    public DateTime ReleaseDate { get; set; }
}