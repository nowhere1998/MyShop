using Microsoft.AspNetCore.Mvc;
using MyShop.Models;

namespace MyShop.Controllers.Components
{
    public class PopupViewComponent : ViewComponent
    {
        private readonly DbMyShopContext _context;

        public PopupViewComponent(DbMyShopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var popup = _context.Advertises
                .Where(x => 
                    x.Position == 8
                    && x.Active == true    
                )
                .FirstOrDefault() ?? new Advertise();
            return View("Default", popup);
        }
    }
}
