namespace WebApplicationrRider.Entity;

public class Actor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string Surname { get; set; } = null!;

    public ICollection<ActorFilm> FilmsActor { get; set; } = new List<ActorFilm>();

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    public DateTime DateDelete { get; set; }

    public void AddFilm(Film film)
    {
        FilmsActor.Add(new ActorFilm
        {
            FkActor = Id,
            FkFilm = film.Id,
            Actor = this,
            Film = film
        });
    }

    public void DeleteAllFilms()
    {
        var filmsActor = FilmsActor.ToList();
        filmsActor.ForEach(x => FilmsActor.Remove(x));
    }
}