using System.Collections.Generic;
using System.Threading.Tasks;
using Dws.Challenge.Application.Dtos.Output;

namespace Dws.Challenge.Application.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistsSummaryOutputDto>> GetArtistsSummaryAsync(string artistName);

        Task<ArtistDetailsOutputDto> GetArtistDetailAsync(string artistId);
    }
}