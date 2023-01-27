namespace WebApplicationrRider.Models.Entity;

public class EarningSale
{
 
    public int Id { get; set; }
   
    
    public int PriceSingleSale { get; set; }
    
    public int SaleAmount { get; set; }
    
    //relation with EaringSale one-to-one
    public int FkFilm { get; set; }
    public Film Film { get; set; }
    
    
    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}