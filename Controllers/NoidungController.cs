using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class NoidungController : Controller
    {
        private readonly DbMyShopContext _context;
        public NoidungController(DbMyShopContext context) 
        {
            _context = context;
        }
        [Route("noi-dung/{slug}")]
        public IActionResult Index(string slug = "")
        {
            var page = _context.Pages
                .Where(x => x.Tag == slug && x.Active == 1)
                .FirstOrDefault() ?? new Page();
            return View(page);
        }
    }
}
