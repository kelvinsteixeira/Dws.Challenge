using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dws.Challenge.Application.Dtos.Output;
using Dws.Challenge.Application.Services;
using Dws.Challenge.Domain.Models;
using Dws.Challenge.Infrastructure.Caching.Interfaces;
using Dws.Challenge.Infrastructure.Gateways.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dws.Challenge.UnitTests.Application
{
    public class ArtistServiceTests
    {
        private readonly ArtistService sut;
        private readonly Mock<IArtistGateway> artistGatewayMock;
        private readonly Mock<ICache> cacheMock;
        private readonly Mock<IMapper> mapperMock;

        public ArtistServiceTests()
        {
            this.artistGatewayMock = new Mock<IArtistGateway>();
            this.mapperMock = new Mock<IMapper>();
            this.cacheMock = new Mock<ICache>();

            this.sut = new ArtistService(this.artistGatewayMock.Object, this.mapperMock.Object, this.cacheMock.Object);
        }

        [Fact]
        public async void Should_GetArtistDetail_From_Cache()
        {
            // arrange
            var artist = new Artist { Id = "id" };
            var artistDto = new ArtistDetailsOutputDto();

            this.cacheMock
                .Setup(c => c.Get<Artist>(It.IsAny<string>()))
                .Returns(artist);

            this.mapperMock
                .Setup(m => m.Map<ArtistDetailsOutputDto>(It.IsAny<Artist>()))
                .Returns(artistDto);

            // act
            var result = await sut.GetArtistDetailAsync(artist.Id);

            // Assert
            this.cacheMock.Verify(c => c.Get<Artist>(artist.Id), Times.Once);
            this.mapperMock.Verify(m => m.Map<ArtistDetailsOutputDto>(artist), Times.Once);
            result.Should().Be(artistDto);
        }

        [Fact]
        public async void Should_GetArtistDetail_After_Fetching()
        {
            // arrange
            var artistToLookFor = new Artist { Id = "1" };
            IEnumerable<Artist> artists = new List<Artist> { artistToLookFor, new Artist { Id = "2" }, new Artist { Id = "3" } };
            var artistDto = new ArtistDetailsOutputDto();

            this.cacheMock
                .Setup(c => c.Get<Artist>(It.IsAny<string>()))
                .Returns((Artist)null);

            this.artistGatewayMock
                .Setup(ag => ag.GetArtistsAsync())
                .ReturnsAsync(() =>
                {
                    this.cacheMock
                        .Setup(c => c.Get<Artist>(artistToLookFor.Id))
                        .Returns(artistToLookFor);

                    return artists;
                });

            this.mapperMock
                .Setup(m => m.Map<ArtistDetailsOutputDto>(It.IsAny<Artist>()))
                .Returns(artistDto);

            // act
            var result = await this.sut.GetArtistDetailAsync(artistToLookFor.Id);

            // Assert
            this.cacheMock.Verify(c => c.Get<Artist>(artistToLookFor.Id), Times.Exactly(2));
            this.cacheMock.Verify(c => c.Save("ArtistsCacheKey", artists), Times.Once);

            foreach (var artist in artists)
            {
                this.cacheMock.Verify(c => c.Save(artist.Id, artist), Times.Once);
            }

            this.artistGatewayMock.Verify(ag => ag.GetArtistsAsync(), Times.Once);
            this.mapperMock.Verify(m => m.Map<ArtistDetailsOutputDto>(artistToLookFor), Times.Once);
            result.Should().Be(artistDto);
        }

        [Fact]
        public async void Should_GetArtistsSummary_From_Cache_And_Fetch()
        {
            // arrange
            IEnumerable<Artist> artists = new List<Artist>
            {
                new Artist { Id = "1" },
                new Artist { Id = "2" },
                new Artist { Id = "3" }
            };

            this.cacheMock
                .Setup(c => c.Get<IEnumerable<Artist>>(It.IsAny<string>()))
                .Returns((IEnumerable<Artist>)null);

            this.artistGatewayMock
                .Setup(ag => ag.GetArtistsAsync())
                .ReturnsAsync(() =>
                {
                    this.cacheMock
                        .Setup(c => c.Get<IEnumerable<Artist>>("ArtistsCacheKey"))
                        .Returns(artists);

                    return artists;
                });

            // act
            var result = (await sut.GetArtistsSummaryAsync(string.Empty)).ToList();

            // Assert
            this.cacheMock.Verify(c => c.Get<IEnumerable<Artist>>("ArtistsCacheKey"), Times.Exactly(2));
            this.cacheMock.Verify(c => c.Save("ArtistsCacheKey", artists), Times.Once);
            this.artistGatewayMock.Verify(ag => ag.GetArtistsAsync(), Times.Once);
            this.mapperMock.Verify(m => m.Map<ArtistsSummaryOutputDto>(It.IsNotNull<Artist>()), Times.Exactly(artists.Count()));
        }
    }
}