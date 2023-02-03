﻿using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models;

public class Genre
{
    [Key] public int Id { get; set; }


    public string? Name { get; set; }

    //[System.Text.Json.Serialization.JsonIgnore]
    public  List<Film> Films { get; set; } = new List<Film>();
    
    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }
}