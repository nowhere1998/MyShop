using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
	public class TintucController : Controller
	{
		private readonly DbMyShopContext _context;

        public TintucController(DbMyShopContext context)
        {
            _context = context;
        }
        [Route("tin-tuc")]
		[Route("tin-tuc/page/{page:int}")]
		public IActionResult Index(int page = 1)
		{
			int pageSize = 6;

			var query = _context.News
				.Where(x => x.Status == 1)
				.OrderByDescending(x => x.Id);

			int totalItems = query.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var news = query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;

			return View(news);
		}

		[Route("chi-tiet-tin-tuc/{slug}")]
        public IActionResult Chitiet(string slug = "")
        {
			var post = new News();
			if(string.IsNullOrEmpty(slug))
			{
				post = _context.News.Where(x => x.Status == 1).FirstOrDefault(post => post.Slug == slug);
			}
            return View("chi-tiet-tin-tuc", post);
        }
    }
}
