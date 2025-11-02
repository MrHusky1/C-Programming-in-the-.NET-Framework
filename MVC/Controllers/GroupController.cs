using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
