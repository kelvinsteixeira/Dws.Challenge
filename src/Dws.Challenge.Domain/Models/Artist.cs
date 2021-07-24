using System.Collections.Generic;

namespace Dws.Challenge.Domain.Models
{
    public class Artist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Genre { get; set; }

        public string Biography { get; set; }

        public long NumPlays { get; set; }

        public List<string> Albums { get; set; }

        public List<List<Album>> AlbumList { get; set; }
    }
}