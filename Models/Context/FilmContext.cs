using Microsoft.EntityFrameworkCore;


namespace WebApplicationrRider.Models;

public class FilmContext : DbContext
{
    public FilmContext(DbContextOptions<FilmContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Film>().HasIndex(u => u.Title).IsUnique();
        builder.Entity<Genre>().HasIndex(u => u.Name).IsUnique();
    }
}