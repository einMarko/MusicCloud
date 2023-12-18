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
    [Table("Genre")]
    public class Genre : IEntity<int>
    {
        #pragma warning disable CS8618
        protected Genre() { }
        #pragma warning restore CS8618
        public Genre(string name)
        {
            Name = name;
            Guid = Guid.NewGuid();
        }
        [Key]
        public int Id { get; private set;}
        public String Name { get; set; }
        public Guid Guid { get; private set; }
    }
}
