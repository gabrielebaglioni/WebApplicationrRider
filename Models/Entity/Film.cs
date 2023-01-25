
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using StringLengthAttribute = System.ComponentModel.DataAnnotations.StringLengthAttribute;

namespace WebApplicationrRider.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string GenreName { get; set; }
        public DateTime ReleaseDate { get; set; }
        
        //relation whit Genre
       // public int GenreId { get; set; }
        
        //[System.Text.Json.Serialization.JsonIgnore]
        //public Genre? Genre { get; set; }
        
        public DateTime DateAdded { get; set; }
        
        public DateTime DateUpdated { get; set; }
        
        public DateTime DateDelete { get; set; }
    }
}
    
   
    

