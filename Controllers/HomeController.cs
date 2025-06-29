using Microsoft.AspNetCore.Mvc;

namespace vishwashakti.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostReviews()
        {
            return Ok("");
        }
    }
}
