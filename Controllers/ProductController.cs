using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
	public class ProductController : Controller
	{
		[Route("san-pham")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
