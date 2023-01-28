using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Models.DTOs.Incoming;

public class GenreSaveDto
{
    [Required] [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the Genre")]
    [StringLength(40)]
    public string? Name { get; set; }

    public static explicit operator GenreSaveDto(Genre entity)
    {
        return new GenreSaveDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}