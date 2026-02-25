using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
	public class GiaiphapController : Controller
	{
		[Route("giai-phap")]
		[Route("giai-phap/{slug}")]
		public IActionResult Index(string slug = "")
		{
			if (string.IsNullOrEmpty(slug))
				return View("Index");

			var viewPath = $"~/Views/Giaiphap/{slug}.cshtml";

			if (!System.IO.File.Exists(
				Path.Combine(Directory.GetCurrentDirectory(), "Views", "Giaiphap", $"{slug}.cshtml")
			))
			{
                return View("Index");
            }

			return View(slug);
		}

		/*[Route("giai-phap-chi-tiet/{slug}")]
		public IActionResult Chitiet(string slug = "")
		{
			if (string.IsNullOrEmpty(slug))
				return View("Index");

			var viewPath = $"~/Views/GiaiPhap/{slug}.cshtml";

			if (!System.IO.File.Exists(
				Path.Combine(Directory.GetCurrentDirectory(), "Views", "GiaiPhap", $"{slug}.cshtml")
			))
			{
				return View("Index");
			}

			return View(slug);
		}*/
	}
}
