namespace WebApplicationrRider.Domain.Exceptions;

public class CheckException : Exception
{
    public CheckException(string errorMessage) : base(errorMessage)
    { }
    
}
public class FilmTitleNotValidException : CheckException
{
    public FilmTitleNotValidException(string title) : base($"Il titolo del film '{title}' non è valido")
    { }
    
}
public class ActorNameAndSurnameNotValidException : CheckException
{
    public ActorNameAndSurnameNotValidException(string name, string surname) : base($"Il nome e cognome dell'attore '{name} {surname}' non sono validi")
    { }
    
}