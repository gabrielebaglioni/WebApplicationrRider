using System.ComponentModel.DataAnnotations;

namespace WebApplicationrRider.Entity;

public class Film
{
    [Key] 
    public int Id { get; set; }

    public string Title { get; set; } = null!;

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
        ActorsFilm.Add(new ActorFilm
        {
            FkActor = actor.Id,
            FkFilm = Id,
            Actor = actor,
            Film = this
        });
    }


    public void DeleteAllActors()
    {
        var actorsFilm = ActorsFilm.ToList();
        actorsFilm.ForEach(x => ActorsFilm.Remove(x));
    }

   
}
