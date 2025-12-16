using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers.Components
{
	public class NavViewComponent : ViewComponent
	{
		private readonly DbMyShopContext _context;
        public NavViewComponent(DbMyShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
		{
			var parentCategories = _context.Categories
				.OrderByDescending(c => c.Id)
				.Where(c => c.ParentId == null)
				.ToList();
			var categories = _context.Categories
				.OrderByDescending(c => c.Id)
				.Where(c => c.ParentId != null)
				.ToList();

			ViewBag.Categories = categories;
			ViewBag.ParentCategories = parentCategories;
			return View("Default");
		}
	}
}
