using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class LienheController : Controller
    {
		private readonly DbMyShopContext _context;
        public LienheController(DbMyShopContext context)
        {
            _context = context;
        }
		[Route("/lien-he")]
        [Route("/noi-dung/lien-he")]
		[HttpGet]
        public IActionResult Index()
        {
			var lienhe = _context.Pages.OrderBy(x => x.Ord).FirstOrDefault(x => x.Tag == "lien-he" && x.Active == 1);
			ViewBag.Lienhe = lienhe;
            return View();
        }

		[Route("lien-he")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Add(Contact model)
		{
			_context.Contacts.Add(model);
			_context.SaveChanges();
			TempData["Success"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.";
			return RedirectToAction("Index");
		}
	}
}
