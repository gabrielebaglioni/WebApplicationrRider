namespace WebApplicationrRider.Models
{
    public class Film
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        
        public DateTime ReleaseDate { get; set; }
        
        //relation with Genre one-to-many
        public int FkGenre { get; set; }
        //[ForeignKey("FK_Genre")]
        public Genre? Genre { get; set; }
    
        //relation with EaringSale one-to-one
        public EarningSale ? EarningSale { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateDelete { get; set; }
        
    }
}