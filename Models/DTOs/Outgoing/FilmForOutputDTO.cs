namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class FilmForOutputDTO
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string GenreName { get; set; }

    public DateTime ReleaseDate { get; set; }
    
    
    public static explicit operator FilmForOutputDTO?(Film? entity)
    {
        if (entity == null)
            return null;

        return new FilmForOutputDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            ReleaseDate = entity.ReleaseDate
            //Genre = entity.Genre == null ? null : (GenreDTO)entity.Genre
        };
    }
}