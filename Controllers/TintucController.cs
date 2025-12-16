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
	}
}
