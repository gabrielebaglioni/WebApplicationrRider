using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApplicationrRider.Models.DTOs;
using WebApplicationrRider.Models.DTOs.Incoming;

namespace WebApplicationrRider.Models.Entity;

public class Film
{
    // [Key] 
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTime ReleaseDate { get; set; }

    //relation with Genre one-to-many
    public int FkGenre { get; set; }

    //[ForeignKey("FK_Genre")]
    public Genre? Genre { get; set; }

    //relation with EaringSale one-to-one
    public EarningSale? EarningSale { get; set; }

    public ICollection<ActorFilm> ActorsFilm { get; set; } = new List<ActorFilm>();

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }

    public void AddActor(Actor actor)
    {
        this.ActorsFilm.Add(new ActorFilm
        {
            FkActor = actor.Id,
            FkFilm = this.Id,
            Actor = actor,
            Film = this
        });
    }
    

    public void DeleteAllActors()
    {
        var actorsFilm = this.ActorsFilm.ToList();
        actorsFilm.ForEach(x => this.ActorsFilm.Remove(x));
    }
    
    public void RemoveUnmatchedActors(List<ActorDto> actorsDto)
    {
        // Recuperare tutti i record di ActorFilm che non corrispondono agli attori presenti nella lista actorsDto
        var actorsToRemove = ActorsFilm.Where(af => !actorsDto.Any(aDto => aDto.Id == af.FkActor)).ToList();

        // Rimuovere i record di ActorFilm dalla lista ActorsFilm
        foreach (var actorToRemove in actorsToRemove)
        {
            ActorsFilm.Remove(actorToRemove);
        }
    }

}


/*return Ok(OperationResult.NOK(
    $"Actor not found in the database: Name: {actor.Name}, Surname: {actor.Surname}"));*/
    
/* if (film.ActorsFilm.Count != filmSaveDto.Actors.Count || film.ActorsFilm.Any(af => filmSaveDto.Actors.All(dto => dto.Id == af.FkActor)))
        {
            
            film.DeleteAllActors();
                var actors = _dbContext.Actor.AsEnumerable()
                    .Where(e => filmSaveDto.Actors.Any(dto => dto.Name == e.Name && dto.Surname == e.Surname))
                    .AsEnumerable();
                foreach (var actor in actors)
                {
                    film.AddActor(actor);
                }
        }
    */
    
/*public void RemoveActor(List<Actor?> actor)
{
    var actorFilm = this.ActorsFilm.FirstOrDefault(af => af.FkActor == actor.FirstOrDefault()?.Id);
    if (actorFilm != null)
    {
        this.ActorsFilm.Remove(actorFilm);
    }
}*/
//creo un ana funzione che rimuove tutti gli attori  che sono associati al film