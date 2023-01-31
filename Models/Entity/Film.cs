using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    public void RemoveActor(Actor actor)
    {
        this.ActorsFilm.Remove(new ActorFilm
        {
            FkActor = actor.Id,
            FkFilm = this.Id,
            Actor = actor,
            Film = this
        });
    }
}
