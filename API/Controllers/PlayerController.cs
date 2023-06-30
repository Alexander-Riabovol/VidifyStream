using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PlayerController : Controller
    {
        [Route("/player/{fileName}")]
        public IActionResult Index(string fileName)
        {
            var domainName = HttpContext.Request.Host;
            this.ViewBag.FileRoute = $"//{domainName}/api/download/{fileName}";
            return View("~/Pages/Player.cshtml");
        }
    }
}
