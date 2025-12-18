using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
	public class TintucController : Controller
	{
		[Route("tin-tuc")]
		public IActionResult Index()
		{
			return View();
		}

        [Route("chi-tiet-tin-tuc")]
        public IActionResult Chitiet()
        {
            return View("chi-tiet-tin-tuc");
        }
    }
}
