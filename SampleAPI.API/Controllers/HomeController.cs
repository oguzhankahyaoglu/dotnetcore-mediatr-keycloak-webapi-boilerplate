using Microsoft.AspNetCore.Mvc;

namespace SampleAPI.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectPermanent("swagger");
    }
}
