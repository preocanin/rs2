using Microsoft.AspNetCore.Mvc;

namespace rs2.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        // GET: / 
        [HttpGet]
        public ActionResult Get()
        {
            return View("~/View/index.html");
        }
    }
}
