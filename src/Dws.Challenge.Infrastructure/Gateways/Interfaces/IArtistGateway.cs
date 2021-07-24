using System.Collections.Generic;
using System.Threading.Tasks;
using Dws.Challenge.Domain.Models;

namespace Dws.Challenge.Infrastructure.Gateways.Interfaces
{
    public interface IArtistGateway
    {
        Task<IEnumerable<Artist>> GetArtistsAsync();
    }
}