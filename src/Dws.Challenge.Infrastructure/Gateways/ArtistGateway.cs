using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dws.Challenge.Domain.Models;
using Dws.Challenge.Infrastructure.Gateways.Interfaces;
using Newtonsoft.Json;

namespace Dws.Challenge.Infrastructure.Gateways
{
    public class ArtistGateway : IArtistGateway
    {
        private const string ClientName = "artist-http-client";
        private readonly IHttpClientFactory httpClientFactory;

        public ArtistGateway(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Artist>> GetArtistsAsync()
        {
            const string resource = "full";

            var client = this.httpClientFactory.CreateClient(ClientName);
            var response = await client.GetAsync(resource);

            return await this.ProcessResponseAsync(response);
        }

        private async Task<IEnumerable<Artist>> ProcessResponseAsync(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Artist>>(responseAsString);
        }
    }
}