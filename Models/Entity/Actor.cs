using WebApplicationrRider.Models.Entity;

namespace WebApplicationrRider.Models;

public class Actor
{
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }
        
        public  string Surname { get; set; }

        public ICollection<ActorFilm> FilmsActor { get; set; } = new List<ActorFilm>();
        
        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateDelete { get; set; }
}