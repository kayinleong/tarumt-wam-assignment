using Microsoft.AspNetCore.Mvc;

namespace Tarumt.WAM.Assignment.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
