using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApplicationrRider.Entity;

public class User
{
    [Key] public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;

    [JsonIgnore] public string Password { get; set; } = null!;


    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}