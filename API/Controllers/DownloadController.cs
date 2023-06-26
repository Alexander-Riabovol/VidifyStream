using Logic.Services.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/download")]
    [ApiController]
    [AllowAnonymous]
    public class DownloadController : ControllerBase
    {
        private readonly IFileService _fileService;

        public DownloadController(IFileService fileService) 
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> Get(string fileName) 
        {
            var response = await _fileService.Download(fileName);
            if(response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return File(response.Content.Data, response.Content.ContentType);
        }
    }
}
