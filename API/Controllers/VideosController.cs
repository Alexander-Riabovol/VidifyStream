using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Logic.CQRS.Video.Commands.Delete;
using VidifyStream.Logic.CQRS.Video.Commands.Post;
using VidifyStream.Logic.CQRS.Video.Commands.Put;
using VidifyStream.Logic.CQRS.Video.Queries.Get;

namespace VidifyStream.API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ISender _mediator;

        public VideosController(ISender mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{videoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<VideoGetDTO>> Get(int videoId)
        {
            var response = await _mediator.Send(new GetVideoQuery(videoId));
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
            var response = await _mediator.Send(new PostVideoCommand(videoPostDTO));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPut]
        [Authorize("user+")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Put([FromForm] VideoPutDTO videoPutDTO)
        {
            var response = await _mediator.Send(new PutVideoCommand(videoPutDTO));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("{videoId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Delete(int videoId)
        {
            var response = await _mediator.Send(new DeleteVideoCommand(videoId, false));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("admin/{videoId}")]
        [Authorize(Policy = "admin-only")]
        public async Task<IActionResult> DeleteAdmin(int videoId)
        {
            var response = await _mediator.Send(new DeleteVideoCommand(videoId, true));
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
