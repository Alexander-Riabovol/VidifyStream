﻿using Data.Dtos.Comment;
using Logic.Services.CommentService;
using Logic.Services.ValidationService;
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
        [Route("video")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> Post(CommentPostDTO commentPostDto)
        {
            var validationResult = await _validationService.Validate(commentPostDto);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _commentService.PostComment(commentPostDto);
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        [Route("replies")]
        [Authorize(Policy = "user+")]
        public async Task<IActionResult> PostReply(ReplyPostDTO replyPostDTO)
        {
            var validationResult = await _validationService.Validate(replyPostDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _commentService.PostReply(replyPostDTO);
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
