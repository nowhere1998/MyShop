using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using Newtonsoft.Json;

namespace MyShop.Controllers
{
	public class CartController : Controller
	{
		private readonly DbMyShopContext _context;
		private List<Cart> carts = new List<Cart>();

        public CartController(DbMyShopContext context)
        {
            _context = context;
        }
        [Route("/cart")]
		public IActionResult Cart()
		{
			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				carts = JsonConvert.DeserializeObject<List<Cart>>(cart);
			}
			return View("Index", carts);
		}

		public IActionResult Add(long id, int quantity = 1)
		{
			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				carts = JsonConvert.DeserializeObject<List<Cart>>(cart);
			}
			if (carts.Any(x => x.Id == id))
			{
				carts.Where(x => x.Id == id).First().Quantity += quantity;
			}
			else
			{
				var test = id;
				var product = _context.Products.Find(id);
				if (product != null)
				{
					var item = new Cart { Id = product.Id, Name = product.Name, Slug = product.Slug, Image = product.Image, Price = product.SalePrice <= 0 ? product.Price : product.SalePrice, Quantity = quantity };
					carts.Add(item);
				}
			}
			HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));

			return RedirectToAction("Cart", "Cart");
		}

		[HttpPost]
		public IActionResult Update(IFormCollection form)
		{
			var cartJson = HttpContext.Session.GetString("cart");
			if (string.IsNullOrEmpty(cartJson))
				return RedirectToAction("Cart");

			var carts = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

			foreach (var key in form.Keys)
			{
				// key = productId
				if (!int.TryParse(key, out int productId))
					continue;

				if (!int.TryParse(form[key], out int quantity))
					continue;

				var item = carts.FirstOrDefault(x => x.Id == productId);
				if (item != null)
				{
					if (quantity <= 0)
						carts.Remove(item);
					else
						item.Quantity = quantity;
				}
			}

			HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
			return RedirectToAction("Cart");
		}

		public IActionResult Remove(long id)
		{
			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				carts = JsonConvert.DeserializeObject<List<Cart>>(cart);
			}
			if (carts.Any(x => x.Id == id))
			{
				var item = carts.Where(x => x.Id == id).First();
				carts.Remove(item);
				HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
			}
			return RedirectToAction("Cart", "Cart");
		}

		public IActionResult Clear()
		{
			HttpContext.Session.Remove("cart");
			return RedirectToAction("Menu", "Home");
		}

		[Route("/checkout")]
		public IActionResult Checkout()
		{
			return View("checkout");
		}

        [HttpPost]
        [Route("/checkout")]
        public IActionResult Checkout(Order order)
        {
            var cartJson = HttpContext.Session.GetString("cart");

            if (string.IsNullOrEmpty(cartJson))
                return RedirectToAction("Cart");

            var carts = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

            if (carts == null || carts.Count == 0)
                return RedirectToAction("Cart");

            // ===== TÍNH TIỀN =====

            decimal subtotal = 0;

            foreach (var item in carts)
            {
                subtotal += (item.Price ?? 0) * item.Quantity;
            }

            // ===== TẠO ORDER =====

            order.OrderCode =
                "ORD" + DateTime.Now.Ticks;

            order.CreatedAt =
                DateTime.Now;

            order.SubtotalAmount =
                subtotal;

            order.TotalAmount =
                subtotal;

            order.OrderStatus =
                "Pending";

            order.PaymentStatus =
                "Unpaid";

            _context.Orders.Add(order);
            _context.SaveChanges();

            // ===== ORDER DETAIL =====

            foreach (var item in carts)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                    Total = item.Price * item.Quantity,
                    CreatedAt = DateTime.Now
                };

                _context.OrderDetails.Add(detail);
            }

            _context.SaveChanges();

            // ===== XÓA CART =====

            HttpContext.Session.Remove("cart");

            // ===== THÔNG BÁO =====

            TempData["SuccessMessage"] =
                "Đặt hàng thành công! Chúng tôi sẽ liên hệ với bạn sớm.";

            // ===== QUAY VỀ TRANG CHỦ =====

            return RedirectToAction("Index", "Home");
        }
    }
}
