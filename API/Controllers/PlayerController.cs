using Logic.Services.Videos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IVideoService _videoService;

        public PlayerController(IVideoService videoService) 
        {
            _videoService = videoService;
        }

        [Route("/player/{videoId}")]
        [AllowAnonymous]
        // This will hide the method from swagger, preventing it from crushing.
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PlayVideoById(int videoId)
        {
            var serviceResponse = await _videoService.GetVideo(videoId);
            this.ViewBag.FileRoute = serviceResponse.Content?.SourceUrl;
            this.ViewBag.Title = serviceResponse.Content?.Title;
            return View("~/Pages/Player.cshtml");
        }
    }
}
