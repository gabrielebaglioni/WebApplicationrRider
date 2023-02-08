using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Controllers.Support;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Services;

namespace WebApplicationrRider.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GenresController : BaseController
{
    private readonly IGenreService _genreService;


    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    //GET: api/Genres
    [HttpGet]
    public async Task<ActionResult> GetAllGenres()
    {
        /*return await TryCatch(async () =>
        {*/
            var genres = await _genreService.GetListAsync();
            return Ok(genres);
        /*});*/
    }


    //GET: api/Genres/5
    //GET A SINGLE GENRE BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreOutputDto>> GetGenreById(int id)
    {
        /*return await TryCatch(async () =>
        {*/
            var genre = await _genreService.Get(id);
            return Ok(genre);
        /*});*/
    }

    //POST: api/Genres
    [HttpPost]
    public async Task<ActionResult<OperationResult>> PostGenre(GenreSaveDto genreSaveDto)
         {
             /*return await TryCatch(async ()  =>
             {*/
             var result = await _genreService.CreateAsync(genreSaveDto); 
             return Ok(GenreSavingOperationResult.OK(result, "Genere aggiunto con successo nel database."));
             /*});*/
         }
    
   

    // //PUT: api/Genres/5
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> PutGenre(int id, GenreSaveDto genreSaveDto)
    {
        
        /*return await TryCatch(async () =>
        {*/
            var result = await _genreService.UpdateAsync(id, genreSaveDto);
            return Ok(GenreSavingOperationResult.OK(result, "Genere aggiornato con successo nel database."));
        /*});*/
    }

    // //DELETE: api/Films/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        /*return await TryCatch(async () =>
        {*/
            var result = await _genreService.DeleteAsync(id);
            return Ok(GenreSavingOperationResult.OK(result, "Genere eliminato con successo dal database."));
        /*});*/
    }
}