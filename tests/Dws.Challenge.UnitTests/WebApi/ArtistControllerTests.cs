using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dws.Challenge.Application.Dtos.Output;
using Dws.Challenge.Application.Services.Interfaces;
using Dws.Challenge.Infrastructure.Logging.Interfaces;
using Dws.Challenge.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dws.Challenge.UnitTests.WebApi
{
    public class ArtistControllerTests
    {
        private readonly ArtistController sut;
        private readonly Mock<ILoggerWrapper<ArtistController>> loggerMock;
        private readonly Mock<IArtistService> artistServiceMock;

        public ArtistControllerTests()
        {
            this.loggerMock = new Mock<ILoggerWrapper<ArtistController>>();
            this.artistServiceMock = new Mock<IArtistService>();

            this.sut = new ArtistController(loggerMock.Object, artistServiceMock.Object);
        }

        [Fact]
        public async void Should_Call_GetArtistsSummary()
        {
            // arrange
            const string artistName = "anyArtist";

            var artists = new List<ArtistsSummaryOutputDto>
            {
                new ArtistsSummaryOutputDto { Id = "1", Image = "img1", Name = "name1", NumPlays = 1 }
            };

            this.artistServiceMock
                .Setup(@as => @as.GetArtistsSummaryAsync(It.IsAny<string>()))
                .ReturnsAsync(artists);

            // act
            var actionResult = await sut.GetArtistsSummaryAsync(artistName);

            // assert
            this.artistServiceMock.Verify(@as => @as.GetArtistsSummaryAsync(artistName), Times.Once);
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Should_Call_GetArtistDetails()
        {
            // arrange
            const string artistId = "artistId";

            var artistDetail = new ArtistDetailsOutputDto { };

            this.artistServiceMock
                .Setup(@as => @as.GetArtistDetailAsync(It.IsAny<string>()))
                .ReturnsAsync(artistDetail);

            // act
            var actionResult = await sut.GetArtistDetailsAsync(artistId);

            // assert
            this.artistServiceMock.Verify(@as => @as.GetArtistDetailAsync(artistId), Times.Once);
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CallTo_GetArtistsSummary_Should_Handle_Exception()
        {
            // arrange
            const string artistName = "anyArtist";

            var artists = new List<ArtistsSummaryOutputDto>
            {
                new ArtistsSummaryOutputDto { Id = "1", Image = "img1", Name = "name1", NumPlays = 1 }
            };

            this.artistServiceMock
                .Setup(@as => @as.GetArtistsSummaryAsync(It.IsAny<string>()))
                .Throws<Exception>();

            // act
            Func<Task<IActionResult>> actionResult = async () => await sut.GetArtistsSummaryAsync(artistName);
            var result = await actionResult.Invoke();

            // assert
            this.artistServiceMock.Verify(@as => @as.GetArtistsSummaryAsync(artistName), Times.Once);
            loggerMock.Verify(l => l.LogError("Failed to retrieve the list of artists", It.IsNotNull<Exception>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void CallTo_GetArtistDetails_Should_Handle_Exception()
        {
            // arrange
            const string artistId = "artistId";

            var artistDetail = new ArtistDetailsOutputDto { };

            this.artistServiceMock
                .Setup(@as => @as.GetArtistDetailAsync(It.IsAny<string>()))
                .Throws<Exception>();

            // act
            Func<Task<IActionResult>> actionResult = async () => await sut.GetArtistDetailsAsync(artistId);
            var result = await actionResult.Invoke();

            // assert
            this.artistServiceMock.Verify(@as => @as.GetArtistDetailAsync(artistId), Times.Once);
            loggerMock.Verify(l => l.LogError($"Failed to retrieve artist details. Id: {artistId}", It.IsNotNull<Exception>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}