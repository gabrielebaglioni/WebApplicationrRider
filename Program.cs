using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Domain.Repositories;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.MiddleWhere;
using WebApplicationrRider.Models.Context;
using WebApplicationrRider.Persistence.Repositories;
using WebApplicationrRider.Services;

namespace WebApplicationrRider;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<FilmContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FilmContext")));
        //----------------------------------------------------------------
        // servizi e repository
        builder.Services.AddScoped<IFilmRepository, FilmRepository>();
        builder.Services.AddScoped<IFilmServices, FilmService>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddControllers();
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

        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }
}