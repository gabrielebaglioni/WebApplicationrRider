using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Incoming;

public class GenreSaveDto
{
    [Required] [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the Genre")]
    [StringLength(40)]
    public string Name { get; set; } = null!;

    public static explicit operator Genre(GenreSaveDto dto)
    {
        return new Genre
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}