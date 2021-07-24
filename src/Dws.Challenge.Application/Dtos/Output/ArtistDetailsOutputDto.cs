using System;
using System.Collections.Generic;

namespace Dws.Challenge.Application.Dtos.Output
{
    public class ArtistDetailsOutputDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long NumPlays { get; set; }

        public string Biography { get; set; }

        public string Image { get; set; }

        public List<ArtistAlbumDetailsOutputDto> Albuns { get; set; }
    }

    public class ArtistAlbumDetailsOutputDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        public List<ArtistAlbumTrackDetailsOutputDto> Tracks { get; set; }
    }

    public class ArtistAlbumTrackDetailsOutputDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long Duration { get; set; }
    }
}