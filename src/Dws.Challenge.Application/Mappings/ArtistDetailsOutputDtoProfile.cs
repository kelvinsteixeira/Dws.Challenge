using System.Linq;
using AutoMapper;
using Dws.Challenge.Application.Dtos.Output;
using Dws.Challenge.Domain.Models;

namespace Dws.Challenge.Application.Mappings
{
    public class ArtistDetailsOutputDtoProfile : Profile
    {
        public ArtistDetailsOutputDtoProfile()
        {
            this.CreateMap<Track, ArtistAlbumTrackDetailsOutputDto>();
            this.CreateMap<Album, ArtistAlbumDetailsOutputDto>();
            this.CreateMap<Artist, ArtistDetailsOutputDto>()
                .ForMember(_ => _.Albuns, src => src.MapFrom(artist => artist.AlbumList.SelectMany(al => al)));
        }
    }
}