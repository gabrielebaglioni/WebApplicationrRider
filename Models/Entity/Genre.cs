using System.ComponentModel.DataAnnotations;


namespace WebApplicationrRider.Models
{
    
    public class Genre
    {
        [Required]
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "You have to insert the Genre")]
        [StringLength(40)]
        public string Name { get; set; }
        
        //[System.Text.Json.Serialization.JsonIgnore]
        public  List<Film> Films { get; set; }
    }
}