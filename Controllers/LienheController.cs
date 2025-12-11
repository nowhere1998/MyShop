using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class LienheController : Controller
    {
        [Route("lien-he")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
