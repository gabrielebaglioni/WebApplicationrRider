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
        
        builder.Entity<Film>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.HasIndex(u => u.Title).IsUnique();
            entityBuilder.HasOne<Genre>(f => f.Genre)
                .WithMany(g => g.Films)
                .HasForeignKey(f => f.FK_Genre)
                .HasPrincipalKey(g => g.Id);
        });
        builder.Entity<Genre>().HasIndex(u => u.Name).IsUnique();
    }
}