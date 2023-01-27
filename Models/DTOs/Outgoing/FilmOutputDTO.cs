using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class FilmOutputDto
{
    public int Id { get;set; }

    public string? Title { get; set; }

    public string? GenreName { get; set; }

    public int TotalEaring { get; set; }

    public DateTime ReleaseDate { get; set; }


    public static explicit operator FilmOutputDto(Film? entity)
    {
        if (entity != null)
        {
            var dto = new FilmOutputDto();
            dto.Id = entity.Id;
            dto.Title = entity.Title;
            dto.GenreName = entity.Genre?.Name ?? string.Empty;
            dto.TotalEaring = entity.EarningSale.PriceSingleSale * entity.EarningSale.SaleAmount;
            dto.ReleaseDate = entity.ReleaseDate;
            return dto;
        }

        throw new InvalidOperationException("Entità Film null => Errore");
    }
}