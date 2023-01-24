
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using StringLengthAttribute = System.ComponentModel.DataAnnotations.StringLengthAttribute;

namespace WebApplicationrRider.Models
{
    public class Film
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "You have to insert the title")]
        [StringLength(40)]
        public string? Title { get; set; }
        
        [Display(Name = "Genre")]
        [Required(ErrorMessage = "You have to insert the genre")]
        //[CheckFilmGenreExist]
        [StringLength(40)]
        public string? GenreName { get; set; }
        
        [Required]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true), DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [DateLessThanOrEqualToToday]
        public DateTime ReleaseDate { get; set; }
        
        //relation whit film
        //public int GenreId { get; set; }
       // public Genre? Genre { get; set; }
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
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

    }
    // //--------------------------------- START ValidationAttribute Controllo se esite il genere----------------------------------------------------------------------
    
    // public class CheckFilmGenreExist : ValidationAttribute
    // {
    //     private readonly FilmContext _dbContext;
    //     public override string FormatErrorMessage(string name)
    //     {
    //         return "the genre dosen't exist in the current context";
    //     }
    //     protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
    //     {
    //         var checkFilmGenreExist = _dbContext.Genres.Any(genre => genre.Name.Equals(genre.Name));
    //
    //         if (checkFilmGenreExist)
    //         {
    //             return ValidationResult.Success;
    //         }
    //         else
    //         {
    //             return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    //         }
    //         
    //         
    //     }
    //
    // }
    //------------------------------------- FINISH ValidationAttribute Controllo se esite il genere--------------------------------------------------------------------
    
}
    
   
    

