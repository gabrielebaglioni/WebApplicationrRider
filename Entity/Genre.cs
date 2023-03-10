using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Entity;

public class Genre
{
    [Key] public int Id { get; set; }


    public string Name { get; set; }= null!;

    //[System.Text.Json.Serialization.JsonIgnore]
    public List<Film> Films { get; set; } = new();

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}