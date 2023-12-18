using Bogus;
using Microsoft.EntityFrameworkCore;
using MusicCloud.Application.Infrastructure;
using MusicCloud.Application.Model;
using MusicCloud.Application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicCloud.Application.infrastructure
{
    public class CloudContext : DbContext
    {
        public CloudContext(DbContextOptions opt) : base(opt) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Genre> Genres => Set<Genre>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().HasOne(w => w.Artist);

            modelBuilder.Entity<Song>().HasOne(w => w.Artist);
            modelBuilder.Entity<Song>().HasOne(w => w.Genre);
            modelBuilder.Entity<Song>().HasOne(w => w.Album);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var type = entity.ClrType;
                if (type.GetProperty("Guid") is not null)
                    modelBuilder.Entity(type).HasAlternateKey("Guid");
            }
        }

        public void Seed(ICryptService cryptService)
        {
            Randomizer.Seed = new Random(420);

            var adminSalt = cryptService.GenerateSecret(256);
            var admin = new User(
                username: "admin",
                salt: adminSalt,
                passwordHash: cryptService.GenerateHash(adminSalt, "1234"),
                usertype: Usertype.Admin);
            Users.Add(admin);
            SaveChanges();

            var i = 0;
            var artists = new Faker<Artist>("en").CustomInstantiator(f => {
                var name = f.Person.UserName;
                var salt = cryptService.GenerateSecret(256);
                var username = $"artist{++i:000}";
                return new Artist(
                    artistName: f.Person.UserName,
                    firstName: f.Person.FirstName,
                    lastName: f.Person.LastName,
                    birthday: f.Person.DateOfBirth,
                    manager: new User(
                        username: username,
                        salt: salt,
                        passwordHash: cryptService.GenerateHash(salt, "1234"),
                        usertype: Usertype.Manager));
            })
            .Generate(5)
            .ToList();
            Artists.AddRange(artists);
            SaveChanges();

            var albums = new Faker<Album>("en").CustomInstantiator(f =>
            {
                var artist = f.Random.ListItem(artists);
                return new Album(
                    title: f.Company.CatchPhrase(),
                    artist: artist,
                    releaseDate: f.Date.Between(artist.Birthday.AddYears(18), DateTime.Now));
            })
            .Generate(25)
            .ToList();
            Albums.AddRange(albums);
            SaveChanges();

            var genre = new Faker<Genre>("en").CustomInstantiator(f =>
            
                new Genre(
                    name: f.Music.Genre())
            )
           .Generate(10)
           .ToList();
            Genres.AddRange(genre);
            SaveChanges();

            var songs = new Faker<Song>("en").CustomInstantiator(f => {
                var album = f.Random.ListItem(albums);
                return new Song(
                     title: f.Company.CatchPhrase(),
                     album: album,
                     artist: album.Artist,
                     genre: f.Random.ListItem(genre),
                     releaseDate: album.ReleaseDate,
                     length: f.Random.Int(120, 300));
            })
            .Generate(125)
            .ToList();
            Songs.AddRange(songs);
            SaveChanges();
        }
    }
}
