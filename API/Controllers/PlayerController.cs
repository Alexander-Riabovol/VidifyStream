using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PlayerController : Controller
    {
        [Route("/player/{fileName}")]
        // This will hide the method from swagger, preventing it from crushing.
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index(string fileName)
        {
            var domainName = HttpContext.Request.Host;
            this.ViewBag.FileRoute = $"//{domainName}/api/download/{fileName}";
            return View("~/Pages/Player.cshtml");
        }
    }
}
