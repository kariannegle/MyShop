// handles incoming HTTP requests, processes them, and generates responses

using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        //GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}