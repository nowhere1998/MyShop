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
                .Where(c => c.Products.Any())
                .Skip(0)
                .Take(10)
                .ToList();
            var products = _context.Products
                .ToList();
            ViewBag.Categories = categories;
            ViewBag.Products = products;
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
