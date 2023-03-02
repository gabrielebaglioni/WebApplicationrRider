using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.Helpers;
using WebApplicationrRider.MiddleWhere;
using WebApplicationrRider.Persistence.Context;
using WebApplicationrRider.Persistence.Repositories;
using WebApplicationrRider.Services;

namespace WebApplicationrRider;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Dns.GetHostName()); 
        var builder = WebApplication.CreateBuilder(args);
        ConfigurationManager configuration= builder.Configuration;


        // Add services to the container.
        builder.Services.AddDbContext<FilmContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FilmContext")));
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<FilmContext>();
        // //----------------------------------------------------------------

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["AppSettings:ValidAudience"],
                ValidIssuer = configuration["AppSettings:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Secret"]))
            };

        });

        
        

        //----------------------------------------------------------------

        builder.Services.AddCors();
        builder.Services.AddControllers();
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        // servizi e repository
        builder.Services.AddScoped<IFilmRepository, FilmRepository>();
        builder.Services.AddScoped<IFilmServices, FilmService>();
        builder.Services.AddScoped<IActorRepository, ActorRepository>();
        builder.Services.AddScoped<IActorService, ActorService>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();        /*builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IUserService, UserService>();*/

        // builder.Services.AddSingleton()
        // builder.Services.AddScoped()
        // builder.Services.AddTransient()


        
        // //----------------------------------------------------------------

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseCors(options =>
        {
            options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseAuthentication();
        app.UseAuthorization();
        

        app.MapControllers();
        app.Run();
    }
}