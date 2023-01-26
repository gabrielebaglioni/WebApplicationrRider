using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationrRider.Models;

public class Film
{
    // [Key] 
    public int Id { get; set; }

    public string Title { get; set; }
    
    public DateTime ReleaseDate { get; set; }

    //relation whit Genre
     public int FK_Genre { get; set; }

    //[System.Text.Json.Serialization.JsonIgnore]
    //[ForeignKey("FK_Genre")]
    public Genre? Genre { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}