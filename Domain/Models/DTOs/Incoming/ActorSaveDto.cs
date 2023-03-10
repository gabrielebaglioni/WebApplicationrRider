using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Incoming;

public class ActorSaveDto
{
    [Required] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the name")]
    [StringLength(40)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "You have to insert the surname")]
    [StringLength(40)]
    public string Surname { get; set; } = null!;

    [Required]
    [Display(Name = "Birthdate")]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateTime Birthdate { get; set; }

    public List<FilmSaveDto> Films { get; set; } = new();

    public static explicit operator Actor(ActorSaveDto dto)
    {
        return new Actor
        {
            Name = dto.Name ?? string.Empty,
            Surname = dto.Surname ?? string.Empty,
            Birthdate = dto.Birthdate
        };
    }
}