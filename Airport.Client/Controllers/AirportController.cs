using Airport.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Airport.Client.Controllers
{
    public class AirportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
