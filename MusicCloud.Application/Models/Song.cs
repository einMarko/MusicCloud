using MusicCloud.Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCloud.Application.models
{
    [Table("Song")]
    public class Song : IEntity<int>
    {
        #pragma warning disable CS8618
        protected Song() { }
        #pragma warning restore CS8618
        public Song(string title, Artist artist, Album album, Genre genre, DateTime releaseDate, int length)
        {
            Title = title;
            Artist = artist;
            ArtistId = artist.Id;
            Album = album;
            AlbumId = album.Id;
            Genre = genre;
            GenreId = genre.Id;
            ReleaseDate = releaseDate;
            Length = length;
            Guid = Guid.NewGuid();
        }
        [Key]
        public int Id { get; private set; }
        public string Title { get; set; }
        public virtual Artist Artist { get; set; }
        public int ArtistId { get; set; }
        public virtual Album Album { get; set; }
        public int AlbumId { get; set; }
        public virtual Genre Genre { get; set; }
        public int GenreId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public Guid Guid { get; private set; }
    }
}
