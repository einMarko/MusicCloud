using MusicCloud.Application.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCloud.Application.models
{
    [Table("Album")]
    public class Album : IEntity<int>
    {

        #pragma warning disable CS8618
        protected Album() { }
        #pragma warning restore CS8618
        public Album(string title, DateTime releaseDate, Artist artist)
        {
            Title = title;
            ReleaseDate = releaseDate;
            Artist = artist;
            ArtistId = artist.Id;
            Guid = Guid.NewGuid();
        }
        [Key]
        public int Id { get; private set; }
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid Guid { get; private set; }
        public ICollection<Song> Songs { get; } = new List<Song>();
    }
}
