using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Models.Entity;


namespace WebApplicationrRider.Models;

public class FilmContext : DbContext
{
    public FilmContext(DbContextOptions<FilmContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<EarningSale> EarningSales { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Film>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.HasIndex(u => u.Title).IsUnique();
            entityBuilder.HasOne<Genre>(f => f.Genre)
                .WithMany(g => g.Films)
                .HasForeignKey(f => f.FkGenre)
                .HasPrincipalKey(g => g.Id);
            entityBuilder.HasOne<EarningSale>(f => f.EarningSale)
                .WithOne(es => es.Film)
                .HasForeignKey<EarningSale>(es => es.FkFilm);

        });
        
        builder.Entity<Genre>().HasIndex(u => u.Name).IsUnique();
    }
}