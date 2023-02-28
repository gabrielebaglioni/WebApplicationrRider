using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Domain.Models.DTOs.Outgoing;

public class UserOutputDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public static explicit operator UserOutputDto(User user)
    {
        return new UserOutputDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone
        };
    }
}