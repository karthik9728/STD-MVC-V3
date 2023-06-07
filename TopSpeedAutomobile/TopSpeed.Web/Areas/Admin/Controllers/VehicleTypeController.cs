using Microsoft.AspNetCore.Mvc;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    public class VehicleTypeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
