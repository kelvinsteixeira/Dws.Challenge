using System;
using System.Threading.Tasks;
using Dws.Challenge.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dws.Challenge.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> logger;
        private readonly IArtistService artistService;

        public ArtistController(ILogger<ArtistController> logger, IArtistService artistService)
        {
            this.logger = logger;
            this.artistService = artistService;
        }

        [HttpGet]
        public Task<IActionResult> GetArtistsSummaryAsync([FromQuery] string artistName)
        {
            return this.ExecAndHandleAsync(async () =>
            {
                var artist = await this.artistService.GetArtistsSummaryAsync(artistName);
                return this.Ok(artist);
            }, $"Failed to retrieve the list of artists");
        }

        [Route("{artistId}")]
        [HttpGet]
        public Task<IActionResult> GetArtistDetailsAsync(string artistId)
        {
            return this.ExecAndHandleAsync(async () =>
            {
                var artist = await this.artistService.GetArtistDetailAsync(artistId);
                return this.Ok(artist);
            }, $"Failed to retrieve artist details. Id: {artistId}");
        }

        private async Task<IActionResult> ExecAndHandleAsync(Func<Task<IActionResult>> func, string errMessage)
        {
            try
            {
                return await func?.Invoke();
            }
            catch (Exception e)
            {
                this.logger.LogError(errMessage, e);
                return this.BadRequest(errMessage);
            }
        }
    }
}