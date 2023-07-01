using Data.Dtos.Video;
using Logic.Services.VideoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService) 
        {
            _videoService = videoService;
        }

        [HttpGet]
        [Route("{videoId}")]
        public async Task<ActionResult<VideoGetDTO>> Get(int videoId)
        {
            var response = await _videoService.Get(videoId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }


    }
}
