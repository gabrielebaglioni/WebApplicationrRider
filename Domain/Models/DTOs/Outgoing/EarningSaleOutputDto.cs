using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Outgoing;

public class EarningSaleOutputDto
{
    [Required] public int Id { get; set; }

    public int TotalEaring { get; set; }

    public static explicit operator EarningSaleOutputDto?(EarningSale entity)
    {
        return new EarningSaleOutputDto
        {
            Id = entity.Id,
            TotalEaring = entity.TotalEarning
        };
    }
}