using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Incoming;

public class EarnigSaleSaveDto
{
    public int TotalEarning { get; set; }

    public static explicit operator EarnigSaleSaveDto?(EarningSale entity)
    {
        return new EarnigSaleSaveDto
        {
            TotalEarning = entity.TotalEarning
        };
    }
}