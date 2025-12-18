using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers.Components
{
	public class FooterViewComponent : ViewComponent
	{
		private readonly DbMyShopContext _context;

        public FooterViewComponent(DbMyShopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
		{
			var pageLienket = _context.Pages
				.Where(x => x.Position == 3
					&& !x.Name.StartsWith("chính")
				&& x.Active == 1)
				.ToList();
			var pageChinhsach = _context.Pages
				.Where(x => x.Position == 3
					&& x.Name.StartsWith("chính")
					&& x.Active == 1)
				.ToList();

			ViewBag.PageLienket = pageLienket;
			ViewBag.PageChinhsach = pageChinhsach;
			return View("Default");
		}
	}
}
