
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;


namespace WebApplicationrRider.Models.DTOs.Incoming;

public class EarningSaleSaveDto
{
    [Key]
    [Microsoft.Build.Framework.Required] 
    public int Id { get; set; }
    
    public int TotalEarning { get; set; }

    public static explicit operator EarningSaleSaveDto?(EarningSale entity)
    {
        return new EarningSaleSaveDto
        {
            Id = entity.Id,
            TotalEarning = entity.TotalEarning
        };
    }
}