using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Logic.CQRS.Users.Commands.Delete.Admin;
using VidifyStream.Logic.CQRS.Users.Commands.Delete.Me;
using VidifyStream.Logic.CQRS.Users.Commands.Post.Avatar;
using VidifyStream.Logic.CQRS.Users.Commands.Put;
using VidifyStream.Logic.CQRS.Users.Queries.Get;
using VidifyStream.Logic.CQRS.Users.Queries.Get.Admin;
using VidifyStream.Logic.CQRS.Users.Queries.Get.Me;

namespace VidifyStream.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;

        public UsersController(ISender mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize("user+")]
        public async Task<ActionResult<UserGetMeDTO>> GetMyProfile()
        {
            var response = await _mediator.Send(new GetUserMeQuery());
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserGetDTO>> Get(int userId)
        {
            var response = await _mediator.Send(new GetUserQuery(userId));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("admin/{userId}")]
        [Authorize("admin-only")]
        public async Task<ActionResult<UserAdminGetDTO>> GetAdmin(int userId)
        {
            var response = await _mediator.Send(new GetUserAdminQuery(userId));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPut]
        [Authorize("user+")]
        public async Task<IActionResult> Put(UserPutDTO userPutDTO)
        {
            var response = await _mediator.Send(new PutUserCommand(userPutDTO));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        [Route("pfp")]
        [Authorize("user+")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadProfilePicture([FromForm]UserAvatarPostDTO pfpDTO)
        {
            var response = await _mediator.Send(new PostUserAvatarCommand(pfpDTO));
            if(response.IsError) 
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpDelete]
        [Route("admin/")]
        [Authorize("admin-only")]
        public async Task<IActionResult> DeleteAdmin(UserAdminDeleteDTO userAdminDeleteDTO)
        {
            var response = await _mediator.Send(new DeleteUserAdminCommand(userAdminDeleteDTO));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("profile")]
        [Authorize("user+")]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var response = await _mediator.Send(new DeleteUserMeCommand());
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
