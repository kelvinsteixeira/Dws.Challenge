using System;
using System.Collections.Generic;

namespace Dws.Challenge.Domain.Models
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public DateTimeOffset ReleasedDate { get; set; }

        public string Band { get; set; }

        public List<Track> Tracks { get; set; }
    }
}