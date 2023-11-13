using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Logic.CQRS.Comments.Commands.Delete;
using VidifyStream.Logic.CQRS.Comments.Commands.Post;
using VidifyStream.Logic.CQRS.Comments.Commands.Post.Reply;
using VidifyStream.Logic.CQRS.Comments.Commands.Put;
using VidifyStream.Logic.CQRS.Comments.Commands.Put.Like;
using VidifyStream.Logic.CQRS.Comments.Queries.Get;
using VidifyStream.Logic.CQRS.Comments.Queries.Get.Replies;
using VidifyStream.Logic.CQRS.Comments.Queries.Get.Video;

namespace VidifyStream.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ISender _mediator;

        public CommentsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{commentId}")]
        public async Task<ActionResult<CommentGetDTO>> Get(int commentId)
        {
            var response = await _mediator.Send(new GetCommentQuery(commentId));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("video/{videoId}")]
        public async Task<ActionResult<List<CommentGetDTO>>> GetCommentsByVideoId(int videoId)
        {
            var response = await _mediator.Send(new GetCommentsByVideoQuery(videoId));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("replies/{commentId}")]
        public async Task<ActionResult<List<ReplyGetDTO>>> GetReplies(int commentId)
        {
            var response = await _mediator.Send(new GetRepliesQuery(commentId));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPost]
        [Route("video")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Post(CommentPostDTO commentPostDto)
        {
            var response = await _mediator.Send(new PostCommentCommand(commentPostDto));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPost]
        [Route("replies")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> PostReply(ReplyPostDTO replyPostDTO)
        {
            var response = await _mediator.Send(new PostReplyCommand(replyPostDTO));
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPut]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Put(CommentPutDTO commentPutDTO)
        {
            var response = await _mediator.Send(new PutCommentCommand(commentPutDTO));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPut]
        [Route("like/{commentId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> ToggleLike(int commentId)
        {
            var response = await _mediator.Send(new ToggleLikeCommand(commentId));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("{commentId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var response = await _mediator.Send(new DeleteCommentCommand(commentId, false));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("admin/{commentId}")]
        [Authorize(Policy = "admin-only")]
        public async Task<IActionResult> DeleteAdmin(int commentId)
        {
            var response = await _mediator.Send(new DeleteCommentCommand(commentId, true));
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
