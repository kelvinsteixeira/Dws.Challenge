using AutoMapper;
using Dws.Challenge.Application.Dtos.Output;
using Dws.Challenge.Domain.Models;

namespace Dws.Challenge.Application.Mappings
{
    public class ArtistsSummaryOutputDtoProfile : Profile
    {
        public ArtistsSummaryOutputDtoProfile()
        {
            this.CreateMap<Artist, ArtistsSummaryOutputDto>();
        }
    }
}