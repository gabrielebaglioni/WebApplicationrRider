using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Authorization;
using WebApplicationrRider.Controllers.Support;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Models.DTOs.Outgoing;
using WebApplicationrRider.Domain.Services;

namespace WebApplicationrRider.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilmsController : BaseController
{
    private readonly IFilmServices _filmServices;

    public FilmsController(IFilmServices filmServices)
    {
        _filmServices = filmServices;
    }

    // GET: api/Films
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<FilmOutputDto>>> GetFilms()
    {
        var films = await _filmServices.GetListAsync();
        return Ok(films);
    }

    //GET: api/Films/2
    [HttpGet("{id}")]
    public async Task<ActionResult<FilmOutputDto>> GetFilm(int id)
    {
        var film = await _filmServices.Get(id);
        return Ok(film);
    }

    // //POST: api/Films
    [HttpPost]
    public async Task<ActionResult<OperationResult<FilmOutputDto>>> CreateFilm(FilmSaveDto filmSaveDto)
    {
        var result = await _filmServices.CreateAsync(filmSaveDto);
        return Ok(FilmSavingOperationResult.OK(result, "film aggiunto con successo nel database."));
    }


    // //PUT:api/Films/3
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult<FilmOutputDto>>> PutFilm(int id, FilmSaveDto filmSaveDto)
    {
        var result = await _filmServices.UpdateAsync(id, filmSaveDto);
        return Ok(FilmSavingOperationResult.OK(result, "film aggiornato con successo nel database."));
    }

// //DELETE:api/Films/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var result = await _filmServices.DeleteAsync(id);
        return Ok(FilmSavingOperationResult.OK(result, "film eliminato con successo dal database."));
    }
}