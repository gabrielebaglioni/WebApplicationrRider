﻿using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Models;

public class FilmSaveDTO
{
    [Required] [Key] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the title")]
    [StringLength(40)]
    public string? Title { get; set; }

    [Required(ErrorMessage = "You have to insert the title")]
    [StringLength(40)]
    public string GenreName { get; set; }

    [Required]
    [Display(Name = "Release Date")]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    [DateLessThanOrEqualToToday]
    public DateTime ReleaseDate { get; set; }
    //public GenreDTO? Genre { get; set; }

    public static explicit operator FilmSaveDTO(Film film)
    {
        return new FilmSaveDTO
        {
            Id = film.Id,
            Title = film.Title,
            GenreName = film.Genre.Name,
            ReleaseDate = film.ReleaseDate
        };
    }
    
    

    
    public class DateLessThanOrEqualToToday : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Date value should not be a future date";
        }

        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            var dateValue = objValue as DateTime? ?? new DateTime();

            if (dateValue.Date > DateTime.Now.Date)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            return ValidationResult.Success;
        }
    }
}