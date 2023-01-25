using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace WebApplicationrRider.Models;

public class SaveFilmDTO
{ 
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string GenreName { get; set; }
    
    // public static explicit operator  SaveFilmDTO?(Film? entity)
    // {
    //     if (entity == null)
    //         return null;
    //     
    //     return new FilmDTO()
    //     {
    //         Id = entity.Id,
    //         Title = entity.Title,
    //         ReleaseDate = entity.ReleaseDate,
    //         Genre = entity.Genre == null ? null : (GenreDTO)entity.Genre
    //
    //     };
    // }
}