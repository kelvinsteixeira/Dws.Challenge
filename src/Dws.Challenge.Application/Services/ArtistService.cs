using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dws.Challenge.Application.Dtos.Output;
using Dws.Challenge.Application.Services.Interfaces;
using Dws.Challenge.Domain.Models;
using Dws.Challenge.Infrastructure.Caching.Interfaces;
using Dws.Challenge.Infrastructure.Gateways.Interfaces;

namespace Dws.Challenge.Application.Services
{
    public class ArtistService : IArtistService
    {
        private const string ArtistsCacheKey = "ArtistsCacheKey";

        private readonly IArtistGateway artistGateway;
        private readonly IMapper mapper;
        private readonly ICache cache;

        public ArtistService(IArtistGateway artistGateway, IMapper mapper, ICache cache)
        {
            this.artistGateway = artistGateway;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<ArtistDetailsOutputDto> GetArtistDetailAsync(string artistId)
        {
            var artist = this.cache.Get<Artist>(artistId);
            if (artist == null)
            {
                await this.FetchArtistsAsync();
                artist = this.cache.Get<Artist>(artistId);
            }

            return this.mapper.Map<ArtistDetailsOutputDto>(artist);
        }

        public async Task<IEnumerable<ArtistsSummaryOutputDto>> GetArtistsSummaryAsync(string artistName)
        {
            var artists = await this.InternalGetArtistsAsync();

            if (!string.IsNullOrWhiteSpace(artistName))
            {
                artists = artists.Where(a => a.Name.Contains(artistName, StringComparison.InvariantCultureIgnoreCase));
            }

            return artists?.Select(this.mapper.Map<ArtistsSummaryOutputDto>);
        }

        private async Task<IEnumerable<Artist>> InternalGetArtistsAsync()
        {
            var artists = this.cache.Get<IEnumerable<Artist>>(ArtistsCacheKey);

            if (artists?.Any() != true)
            {
                await this.FetchArtistsAsync();
                artists = this.cache.Get<IEnumerable<Artist>>(ArtistsCacheKey);
            }
            else
            {
                Task.Run(async () => await this.FetchArtistsAsync());
            }

            return artists;
        }

        private async Task FetchArtistsAsync()
        {
            var artists = await this.artistGateway.GetArtistsAsync();
            this.cache.Save(ArtistsCacheKey, artists);

            foreach (var artist in artists)
            {
                if (!this.cache.ContainsKey(artist.Id))
                {
                    this.cache.Save(artist.Id, artist);
                }
            }
        }
    }
}