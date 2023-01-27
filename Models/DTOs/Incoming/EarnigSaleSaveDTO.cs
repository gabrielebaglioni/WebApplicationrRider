using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models.DTOs.Incoming;

public class EarnigSaleSaveDto
{
    [Key]
    [Required] 
    public int Id { get; set; }
    
    public int TotalEaring { get; set; }

    public static explicit operator EarnigSaleSaveDto?(EarningSale entity)
    {
        return new EarnigSaleSaveDto
        {
            Id = entity.Id,
            TotalEaring = entity.PriceSingleSale * entity.SaleAmount
        };
    }
}