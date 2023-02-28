using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplicationrRider.Domain.Exceptions;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.Helpers;
using WebApplicationrRider.Persistence.Context;

namespace WebApplicationrRider.MiddleWhere;

public class JwtMiddleware
{
    private readonly AppSettings _appSettings;
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, FilmContext dbContext)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var validatedToken = ValidateToken(token);
            if (validatedToken != null)
            {
                var userId = int.Parse(validatedToken.Claims.First(x => x.Type == "id").Value);
                var user = userService.Get(userId);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, userId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(claimsIdentity);

                context.User = principal;
            }
        }

        await _next(context);

        await dbContext.DisposeAsync();
    }

    private JwtSecurityToken ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        return validatedToken as JwtSecurityToken ?? throw new CheckException("invalid token");
    }
}