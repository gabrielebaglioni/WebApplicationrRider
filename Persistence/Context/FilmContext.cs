using Microsoft.EntityFrameworkCore;
using WebApplicationrRider.Entity;

namespace WebApplicationrRider.Models.Context;

public class FilmContext : DbContext
{
    public FilmContext(DbContextOptions<FilmContext> options)
        : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;

    public DbSet<Actor> Actor { get; set; } = null!;
    public DbSet<ActorFilm> ActorFilm { get; set; } = null!;


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
        });

        builder.Entity<Actor>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id)
                .UseIdentityColumn();
        });

        builder.Entity<ActorFilm>(entityBuilder =>
        {
            entityBuilder.HasKey(x => new { x.FkActor, x.FkFilm });
            entityBuilder.HasOne<Actor>(af => af.Actor)
                .WithMany(a => a.FilmsActor)
                .HasForeignKey(af => af.FkActor);
            entityBuilder.HasOne<Film>(af => af.Film)
                .WithMany(f => f.ActorsFilm)
                .HasForeignKey(af => af.FkFilm);
        });

        builder.Entity<EarningSale>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.HasOne<Film>(es => es.Film)
                .WithOne(f => f.EarningSale)
                .HasForeignKey<EarningSale>(es => es.FkFilm)
                .OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(x => x.Id)
                .UseIdentityColumn();
        });

        builder.Entity<Genre>().HasIndex(u => u.Name).IsUnique();
    }
}