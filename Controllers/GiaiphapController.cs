using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MyShop.Controllers
{
	public class GiaiphapController : Controller
	{
		private readonly ICompositeViewEngine _viewEngine;
		public GiaiphapController(ICompositeViewEngine viewEngine)
		{
			_viewEngine = viewEngine;
		}

		[Route("giai-phap")]
		[Route("giai-phap/{slug}")]
		public IActionResult Index(string slug = "")
		{
			if (string.IsNullOrEmpty(slug))
				return View("Index");

			var result = _viewEngine.FindView(ControllerContext, slug, false);

			if (!result.Success)
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
