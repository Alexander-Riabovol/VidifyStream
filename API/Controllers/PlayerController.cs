using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Logic.CQRS.Videos.Queries.Get;

namespace VidifyStream.API.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ISender _mediator;

        public PlayerController(ISender mediator) 
        {
            _mediator = mediator;
        }

        [Route("/player/{videoId}")]
        [AllowAnonymous]
        // This will hide the method from swagger, preventing it from crushing.
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PlayVideoById(int videoId)
        {
            var serviceResponse = await _mediator.Send(new GetVideoQuery(videoId));
            this.ViewBag.FileRoute = serviceResponse.Content?.SourceUrl;
            this.ViewBag.Title = serviceResponse.Content?.Title;
            return View("~/Pages/Player.cshtml");
        }
    }
}
