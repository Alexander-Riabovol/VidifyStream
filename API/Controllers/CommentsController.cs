using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Logic.Services.Comments;
using VidifyStream.Logic.Services.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VidifyStream.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IValidationService _validationService;

        public CommentsController(ICommentService commentService, IValidationService validationService) 
        {
            _commentService = commentService;
            _validationService = validationService;
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

        [HttpGet]
        [Route("replies/{commentId}")]
        public async Task<ActionResult<List<ReplyGetDTO>>> GetReplies(int commentId)
        {
            var response = await _commentService.GetReplies(commentId);
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
            var validationResult = await _validationService.ValidateAsync(commentPostDto);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _commentService.PostComment(commentPostDto);
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
            var validationResult = await _validationService.ValidateAsync(replyPostDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _commentService.PostReply(replyPostDTO);
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
            var validationResult = _validationService.Validate(commentPutDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _commentService.Put(commentPutDTO);
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPut]
        [Route("like/{commentId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> ToggleLike(int commentId)
        {
            var response = await _commentService.ToggleLike(commentId);
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("{commentId}")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var response = await _commentService.Delete(commentId);
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("admin/{commentId}")]
        [Authorize(Policy = "admin-only")]
        public async Task<IActionResult> DeleteAdmin(int commentId)
        {
            var response = await _commentService.DeleteAdmin(commentId);
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
