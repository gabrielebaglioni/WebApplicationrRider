



namespace WebApplicationrRider.Models.DTOs.Incoming;

public class EarningSaleSaveDto
{
    
    [Microsoft.Build.Framework.Required]
    public int TotalEarning { get; set; }

    public static explicit operator EarningSaleSaveDto?(EarningSale entity)
    {
        return new EarningSaleSaveDto
        {
            TotalEarning = entity.TotalEarning
        };
    }
}