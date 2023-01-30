namespace WebApplicationrRider.Models.DTOs.Outgoing;

public class ActorOutputDto
{
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthdate { get; set; }


    public static explicit operator ActorOutputDto(Actor actor)
    {
        return new ActorOutputDto
        {
            
            Name = actor.Name ?? string.Empty,
            Surname = actor.Surname ?? string.Empty,
            Birthdate = actor.Birthdate,
            
        };
    }
}
