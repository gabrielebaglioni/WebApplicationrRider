using Microsoft.Build.Framework;

namespace WebApplicationrRider.Domain.Models;

public class AuthenticateRequest
{
    [Required] public string Username { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
}