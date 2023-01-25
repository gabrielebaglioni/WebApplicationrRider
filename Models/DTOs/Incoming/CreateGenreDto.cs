using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Models;

public class CreateGenreDto
{
    [Required(ErrorMessage = "You have to insert the Genre")]
    [StringLength(40)]
    public string? Name { get; set; }
}