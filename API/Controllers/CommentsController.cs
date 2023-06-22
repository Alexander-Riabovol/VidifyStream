using Data.Dtos.Comment;
using Logic.Services.CommentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService) 
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("{commentId}")]
        public async Task<ActionResult<CommentGetDTO>> Get(int commentId)
        {
            var response = await _commentService.GetComment(commentId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("replies/{commentId}")]
        public async Task<ActionResult<List<CommentReplyGetDTO>>> GetReplies(int commentId)
        {
            var response = await _commentService.GetReplies(commentId);
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
            var response = await _commentService.GetCommentsByVideoId(videoId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPost]
        [Route("video/{videoId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Post(int videoId, CommentPostDTO commentDto)
        {
            var response = await _commentService.PostComment(videoId, commentDto);
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        [Route("replies/{commentId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> PostReply(int commentId, CommentPostDTO commentDto)
        {
            var response = await _commentService.PostReply(commentId, commentDto);
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
