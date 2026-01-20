using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using System.Diagnostics;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbMyShopContext _context;

        public HomeController(ILogger<HomeController> logger, DbMyShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories
                .Include(c => c.Parent)
                .Include(c => c.Products)
                .OrderByDescending(c => c.Id)
                .Where(c => c.ParentId != null && c.Products != null)
                .Where(c => c.Products.Any(p => p.Status == "active"))
                .Skip(0)
                .Take(10)
                .ToList();
            var products = _context.Products
                .Where(p => p.Status == "active")
                .ToList();
            var banners = _context.Advertises
                .Where(x => x.Position == 1)
                .OrderBy(x => x.Ord)
                .ToList();
            var news = _context.News
                .OrderByDescending (x => x.Id)
                .Where(x => x.Status == 1)
                .ToList();

            var pagesL1 = _context.Pages
                .Where(l1 =>
                    l1.Level != null &&
                    l1.Level.Length == 5 &&
                    l1.Active == 1 &&
                    (l1.Position == 2 || l1.Position == 4))
                .OrderBy(l1 => l1.Ord)
                .Select(l1 => new PageL1
                {
                    Page = l1,

                    // Có c?p 2
                    HasChild = _context.Pages.Any(l2 =>
                        l2.Level != null &&
                        l2.Level.Length == 10 &&
                        l2.Level.StartsWith(l1.Level) &&
                        l2.Active == 1 &&
                        (l2.Position == 2 || l2.Position == 4)
                    ),

                })
                .ToList();

            var pagesL2 = _context.Pages
                .OrderBy(x => x.Ord)
                .Where(x => x.Level != null
                    && x.Level.Length == 10
                    && x.Active == 1
                    && (x.Position == 2 || x.Position == 4))
                .ToList();

            ViewBag.PagesL1 = pagesL1;
            ViewBag.PagesL2 = pagesL2;
            ViewBag.News = news;
			ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.Banners = banners;  
            return View();
        }

		public IActionResult Error(int? statusCode)
		{
			ViewBag.StatusCode = statusCode;
			return View();
		}

		public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
