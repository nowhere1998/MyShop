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

            var config = _context.Configs.FirstOrDefault() ?? new Config();

            var pagesL1 = _context.Pages
                .Where(l1 =>
                    l1.Level != null &&
                    l1.Level.Length == 5 &&
                    l1.Active == 1 &&
                    l1.Position == 1)
                .OrderBy(l1 => l1.Ord)
                .Select(l1 => new PageL1
                {
                    Page = l1,

                    // Có cấp 2
                    HasChild = _context.Pages.Any(l2 =>
                        l2.Level != null &&
                        l2.Level.Length == 10 &&
                        l2.Level.StartsWith(l1.Level) &&
                        l2.Active == 1 &&
                        l2.Position == 1
                    ),

                    // Có cấp 3
                    HasChildL3 = _context.Pages.Any(l3 =>
                        l3.Level != null &&
                        l3.Level.Length == 15 &&
                        l3.Level.StartsWith(l1.Level) &&
                        l3.Active == 1 &&
                        l3.Position == 1
                    )
                })
                .ToList();

            var pagesL2 = _context.Pages
                .OrderBy(x => x.Ord)
                .Where(x => x.Level != null
                    && x.Level.Length == 10
                    && x.Active == 1
                    && x.Position == 1)
                .ToList();

            ViewBag.Config = config;
			ViewBag.PageLienket = pageLienket;
			ViewBag.PageChinhsach = pageChinhsach;
			return View("Default");
		}
	}
}
