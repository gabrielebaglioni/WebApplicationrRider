namespace WebApplicationrRider.Entity;

public class ActorFilm
{
    public int FkActor { get; set; }
    public Actor? Actor { get; set; }

    public int FkFilm { get; set; }
    public Film? Film { get; set; }
}