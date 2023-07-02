using Data.Dtos.Video;
using Logic.Services.ValidationService;
using Logic.Services.VideoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IValidationService _validationService;

        public VideoController(IVideoService videoService, IValidationService validationService) 
        {
            _videoService = videoService;
            _validationService = validationService;
        }

        [HttpGet]
        [Route("{videoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<VideoGetDTO>> Get(int videoId)
        {
            var response = await _videoService.GetVideo(videoId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPost]
        [Authorize("user+")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm]VideoPostDTO videoPostDTO)
        {
            var validationResult = _validationService.Validate(videoPostDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _videoService.PostVideo(videoPostDTO);
            return StatusCode(response.StatusCode, response.Message);
        }

    }
}
