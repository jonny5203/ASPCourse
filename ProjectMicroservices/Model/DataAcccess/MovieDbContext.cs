using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.Model.Identity;

namespace ProjectMicroservices.Model.DataAcccess;

public class MovieDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Director> Directors { get; set; }

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });
        
        // Creating many to many relationship with predefined joint table, ef core will actually handle this
        // without the joint model, and just create a joint table, but it's more flexible to have one defined
        modelBuilder.Entity<Movie>()
            .HasMany(n => n.Genres)
            .WithMany(g => g.Movies);
        
        // Movie actor relationship, one actor could star in many movies and one movie may have many actors
        modelBuilder.Entity<Movie>()
            .HasMany(n => n.Actors)
            .WithMany(a => a.Movies);
        
        // One to many relationship, a movie has one director(WithOne) and a director had many movies(HasMany)
        modelBuilder.Entity<Director>()
            .HasMany(d => d.Movies)
            .WithOne(m => m.Directors)
            .HasForeignKey(m => m.DirectorId);
        
        // A movie has many review, a review only has one movie
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Reviews)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId);

        // Old method
        /*// Creating a composite key with GenreId and MovieId, having both as a unique identifier, instead of
        // creating another id column just for id, and also ensure uniqueness in the combination of movies and genres
        modelBuilder.Entity<GenreMovie>()
            .HasKey(gm => new { gm.GenreId, gm.MovieId });

        // Defining one to many relationship with between genremovie and genre, with the genreid as foreign key
        modelBuilder.Entity<GenreMovie>()
            .HasOne(gm => gm.Genre)
            .WithMany(g => g.GenreMovies)
            .HasForeignKey(gm => gm.GenreId);

        // Defining one to many relationship with between genremovie and movie, with the movieid as foreign key
        modelBuilder.Entity<GenreMovie>()
            .HasOne(gm => gm.Movie)
            .WithMany(m => m.GenreMovies)
            .HasForeignKey(gm => gm.MovieId);*/
        
        
    }
    
}