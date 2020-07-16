using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Infrastructure.Data
{

    // Install all the EF Core libaries using Nuget package Manger
    // Create a class that inherits from DbContext class 
    // DbContext kinda represents your Database
    // create a connection a string which EF is gonna use tp create/access the Database, should include server name,  Database Name and any credentials 
    // create an Entity Class, Genre table 
    // Make sure to add your Entity class as a DbSet property inside your DbContext class
    // In EF we have thing called Migrations, we are gonna use Migrations to create our Database
    // We need to add Migration commands to Create the tables, database etc 
    // When running Migration commands, make sure the project selected is the one project which has DbContext class
    // Make sure you add reference for library that has DbContext to your startup project, in this case MVC 
    // Tell MVC project that we are using Entity Framework in startup file 
    // Add DbContext options as constructor parameter for our DbContext
    // Add-Migration MigrationName, make sure your migration names are meaningful, don't use names such as xyz, abc, migration like that
    // Make sure you have Migrations folder created, and check the created migration file
    // After Creating Migration file and verifying it we need to use update-database command
    // Whenever you change your model/entity always make sure you add new Migration
    // With CF approach never change the Database directly, 
    //  always change the entities using DataAnnotations or Fluent API and add new migration  
    //      then updata database


    // In EF we have 2 ways to create our entities and model our database using Code-First approach
    // 1. Data Annotations which is nothing but bunch of C# attributes that we can use
    // 2. Fluent API is more syntax and more powerful and usually uses lambdas
       // Combine both DataAnnotations and Fluent API 



    public class MovieStoreDbContext : DbContext
    {
        private int max;

        // Multiple Dbsets, all the Dbsets you create are gonna reside in your DbContext class

        // This Dbset, is gonna represent out Table based on Entity class which is Genre in this class 


        public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options):base(options)
        {

        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<Cast> Casts { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Trailer>(ConfigureTrailer);
            modelBuilder.Entity<MovieGenre>(ConfigureMovieGenre);
            modelBuilder.Entity<Cast>(ConfigureCast);
            modelBuilder.Entity<Favorite>(ConfigureFavorite);
            modelBuilder.Entity<MovieCast>(ConfigureMovieCast);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);
            modelBuilder.Entity<Review>(ConfigureReview);
            modelBuilder.Entity<Role>(ConfigureRole);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<UserRole>(ConfigureUserRole);
        }


        private void ConfigureCast(EntityTypeBuilder<Cast> modelBuilder)
        {
            modelBuilder.ToTable("Cast");
            modelBuilder.HasKey(c => c.Id );
            modelBuilder.Property(c => c.Name).HasMaxLength(128);
            modelBuilder.Property(c => c.Gender).HasMaxLength(2048);
            modelBuilder.Property(c => c.TmdbUrl).HasMaxLength(4096);
            modelBuilder.Property(c => c.ProfilePath).HasMaxLength(4096);
        }

        private void ConfigureFavorite(EntityTypeBuilder<Favorite> modelBuilder)
        {
            modelBuilder.ToTable("Favorite");
            modelBuilder.HasKey(f => new { f.Id });
            modelBuilder.HasOne(f => f.Movie).WithMany(m => m.Favorites).HasForeignKey(f => f.MovieId);
            modelBuilder.HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId);
        }
        private void ConfigureMovieCast(EntityTypeBuilder<MovieCast> modelBuilder)
        {
            modelBuilder.ToTable("MovieCast");
            modelBuilder.HasKey(mc => new { mc.MovieId, mc.CastId, mc.Character });
            modelBuilder.HasOne(mc => mc.Movie).WithMany(m => m.MovieCasts).HasForeignKey(mc => mc.MovieId);
            modelBuilder.HasOne(mc => mc.Cast).WithMany(c => c.MovieCasts).HasForeignKey(mc => mc.CastId);
        }

       

        private void ConfigureUser(EntityTypeBuilder<User> modelBulider)
        {
            modelBulider.ToTable("User");
            modelBulider.HasKey(u => u.Id);
            modelBulider.Property(u => u.FirstName).HasMaxLength(128);
            modelBulider.Property(u => u.LastName).HasMaxLength(128);
            modelBulider.Property(u => u.Email).HasMaxLength(256);
            modelBulider.Property(u => u.HashedPassword).HasMaxLength(1024);
            modelBulider.Property(u => u.Salt).HasMaxLength(1024);
            modelBulider.Property(u => u.PhoneNumber).HasMaxLength(16);
            modelBulider.Property(u => u.TwoFactorEnabled).HasColumnType("bit");
            modelBulider.Property(u => u.LockoutEndDate).HasColumnType("datetime2");
            modelBulider.Property(u => u.LastLoginDateTime).HasDefaultValueSql("getdate()");
            modelBulider.Property(u => u.IsLocked).HasColumnType("bit");
            modelBulider.Property(u => u.AccessFailedCount).IsRequired();
        }

        private void ConfigureUserRole(EntityTypeBuilder<UserRole> modelBuilder)
        {
            modelBuilder.ToTable("UserRole");
            modelBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId);
            modelBuilder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
        }

        private void ConfigureRole(EntityTypeBuilder<Role> modelBuilder)
        {
            modelBuilder.ToTable("Role");
            modelBuilder.HasKey(r => r.Id);
            modelBuilder.Property(r => r.Name).HasMaxLength(20);
        }

        private void ConfigureReview(EntityTypeBuilder<Review> modelbulider)
        {
            modelbulider.ToTable("Review");
            modelbulider.HasKey(r => new { r.MovieId, r.UserId });
            modelbulider.Property(r => r.Rating).HasColumnType("decimal(3,2)");
            modelbulider.Property(r => r.ReviewText).HasMaxLength(1000000);

        }

       

        private void ConfigurePurchase(EntityTypeBuilder<Purchase> modelBuilder)
        {
            modelBuilder.ToTable("Purchase");
            modelBuilder.HasKey(p => new { p.Id });
            modelBuilder.Property(p => p.UserId).IsRequired();
            modelBuilder.Property(p => p.PurchaseNumber).IsRequired();
            modelBuilder.Property(p => p.TotalPrice).IsRequired().HasColumnType("decimal(5, 2)");
            modelBuilder.Property(p => p.PurchaseDateTime).IsRequired();

        }


        private void ConfigureMovieGenre(EntityTypeBuilder<MovieGenre> modelBuilder)
        {
            modelBuilder.ToTable("MovieGenre");
            modelBuilder.HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.HasOne(mg => mg.Movie).WithMany(g => g.MovieGenres).HasForeignKey(mg => mg.MovieId);
            modelBuilder.HasOne(mg => mg.Genre).WithMany(g => g.MovieGenres).HasForeignKey(mg => mg.GenreId);
        }

        private void ConfigureTrailer(EntityTypeBuilder<Trailer> modelBuilder)
        {
            modelBuilder.ToTable("Trailer");
            modelBuilder.HasKey(t => t.Id);
            modelBuilder.Property(t => t.Name).HasMaxLength(2048);
            modelBuilder.Property(t => t.TrailerUrl).HasMaxLength(2048);
        }

        private void ConfigureMovie(EntityTypeBuilder<Movie> modelBuilder)
        {
            // we can use Fluent API Configurations to model our tables
            modelBuilder.ToTable("Movie");
            modelBuilder.HasKey(m => m.Id);
            modelBuilder.Property(m => m.Title).IsRequired().HasMaxLength(256);

            modelBuilder.Property(m => m.Overview).HasMaxLength(4096);
            modelBuilder.Property(m => m.Tagline).HasMaxLength(512);
            modelBuilder.Property(m => m.ImdbUrl).HasMaxLength(2048);
            modelBuilder.Property(m => m.TmdbUrl).HasMaxLength(2048);
            modelBuilder.Property(m => m.PosterUrl).HasMaxLength(2048);
            modelBuilder.Property(m => m.BackdropUrl).HasMaxLength(2048);
            modelBuilder.Property(m => m.OriginalLanguage).HasMaxLength(64);
            modelBuilder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            modelBuilder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");

            // we don't want to create Rating Column but we want C# rating property in our Entity so that 
            //  we can show Movie rating in the UI 
            modelBuilder.Ignore(m => m.Rating);

        }


    }

}