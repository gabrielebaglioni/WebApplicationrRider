﻿using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Incoming;

public class FilmSaveDto
{
    [Required] public int Id { get; set; }

    [Required(ErrorMessage = "You have to insert the title")]
    [StringLength(40)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "You have to insert the title")]
    [StringLength(40)]
    public string? GenreName { get; set; }

    [Required]
    [Display(Name = "Release Date")]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    [DateLessThanOrEqualToToday]

    public int TotalEarning { get; set; }

    public IEnumerable<Actor?> Actors { get; set; } = new List<Actor?>();
    public DateTime ReleaseDate { get; set; }


    public static explicit operator Film(FilmSaveDto dto)
    {
        return new Film
        {
            Id = dto.Id,
            Title = dto.Title ?? string.Empty,
            ReleaseDate = dto.ReleaseDate,
            EarningSale = new EarningSale { TotalEarning = dto.TotalEarning }
        };
    }

    public class DateLessThanOrEqualToToday : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Date value should not be a future date";
        }

        protected override ValidationResult? IsValid(object? objValue, ValidationContext validationContext)
        {
            var dateValue = objValue as DateTime? ?? new DateTime();

            if (dateValue.Date > DateTime.Now.Date)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            return ValidationResult.Success;
        }
    }
}