using MusicCloud.Application.Model;
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
    [Table("Artist")]
    public class Artist : IEntity<int>
    {
        #pragma warning disable CS8618
        protected Artist() { }
        #pragma warning restore CS8618
        public Artist(string artistName, string firstName, string lastName, DateTime birthday, User? manager = null)
        {
            ArtistName = artistName;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Manager = manager;
            Guid = Guid.NewGuid();
        }
        [Key]
        public int Id { get; private set; }
        public string ArtistName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public DateTime Birthday { get; set; }
        public Guid Guid { get; private set; }
        public int? ManagerId { get; set; }
        public User? Manager { get; set; }
        public ICollection<Album> Albums { get; } = new List<Album>();
        public ICollection<Song> Songs { get; } = new List<Song>();
    }
}
