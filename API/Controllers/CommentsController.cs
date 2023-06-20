using Data.Dtos.Comment;
using Logic.Services.CommentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
        [Route("/replies/{commentId}")]
        public async Task<ActionResult<List<CommentGetDTO>>> GetReplies(int commentId)
        {
            return Ok();
        }
    }
}
