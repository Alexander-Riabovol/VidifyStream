using Logic.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/download")]
    [AllowAnonymous]
    [ApiController]
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

        [HttpGet]
        [Route("blank")]
        public IActionResult GetBlankProfilePicture()
        {
            return File("~/blank.jpg", "image/jpg");
        }
    }
}
