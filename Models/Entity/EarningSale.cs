namespace WebApplicationrRider.Models.Entity;

public class EarningSale
{
 
    public int Id { get; set; }
   
    
    public int TotalEarning { get; set; }
    
    //relation with EaringSale one-to-one
    public int FkFilm { get; set; }
    public Film Film { get; set; } = null!;


    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}