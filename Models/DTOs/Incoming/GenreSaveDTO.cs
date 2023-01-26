using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Models;

public class GenreSaveDTO
{
    [Required] [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the Genre")]
    [StringLength(40)]
    public string? Name { get; set; }

    public static explicit operator GenreSaveDTO(Genre entity)
    {
        return new GenreSaveDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}