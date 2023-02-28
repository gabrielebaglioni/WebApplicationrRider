using System.Net;
using Microsoft.EntityFrameworkCore;
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


        // Add services to the container.
        builder.Services.AddDbContext<FilmContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FilmContext")), ServiceLifetime.Transient);
        // //----------------------------------------------------------------


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
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IUserService, UserService>();

        // builder.Services.AddSingleton()
        // builder.Services.AddScoped()
        // builder.Services.AddTransient()


        // builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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

        app.UseMiddleware<JwtMiddleware>();

        app.MapControllers();
        app.Run();
    }
}