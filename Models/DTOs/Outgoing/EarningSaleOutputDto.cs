using System.ComponentModel.DataAnnotations;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models.DTOs.Outgoing;
public class EarningSaleOutputDto
    {
        [Key]
        [Required] 
        public int Id { get; set; }
    
        public int TotalEaring { get; set; }

        public static explicit operator EarningSaleOutputDto?(EarningSale entity)
        {
            return new EarningSaleOutputDto
            {
                Id = entity.Id,
                TotalEaring = entity.PriceSingleSale * entity.SaleAmount
            };
        }
    }
