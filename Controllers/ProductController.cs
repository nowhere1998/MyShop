using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
	public class ProductController : Controller
	{
		private readonly DbMyShopContext _context;

        public ProductController(DbMyShopContext context)
        {
            _context = context;
        }
        [Route("san-pham")]
		[Route("san-pham/page/{page:int}")]
		[Route("san-pham/{parentSlug}")]
		[Route("san-pham/{parentSlug}/page/{page:int}")]
		[Route("san-pham/{parentSlug}/{slug}")]
		[Route("san-pham/{parentSlug}/{slug}/page/{page:int}")]
		public IActionResult Index(long? minPrice, long? maxPrice, string? orderby, string? parentSlug, string? slug, int page = 1)
		{
			int pageSize = 12;
			int totalItems = 0;
			List<Product> productList = new List<Product>();
			List<Product> products = new List<Product>();
			List<int> childIds = new List<int>();
			// Xác định slug danh mục
			string? catSlug = slug ?? parentSlug;

			IQueryable<Product> query = _context.Products.AsQueryable();

			// Nếu có slug danh mục
			if (!string.IsNullOrEmpty(catSlug))
			{
				var category = _context.Categories.FirstOrDefault(x => x.Slug == catSlug);

				if (category != null)
				{
					// Nếu là danh mục CHA
					if (category.ParentId == null)
					{
						// Lấy tất cả danh mục con của nó
						childIds = _context.Categories
							.Where(x => x.ParentId == category.Id)
							.Select(x => x.Id)
							.ToList();

						// Bao gồm luôn parent category nếu sản phẩm thuộc cha
						childIds.Add(category.Id);

						foreach (var p in _context.Products.ToList())
						{
							if (childIds.Contains(p.CategoryId))
							{
								totalItems++;
							}
						}
					}
					else
					{
						// Nếu là danh mục CON → lọc bình thường
						query = query.Where(x => x.CategoryId == category.Id);
					}
				}
			}

			// Lấy danh sách sản phẩm
			if (!string.IsNullOrEmpty(catSlug) && catSlug.Equals(parentSlug))
			{
				foreach (var p in _context.Products.OrderByDescending(x => x.Id).ToList())
				{
					if (childIds.Contains(p.CategoryId))
					{
						productList.Add(p);
					}
				}
				products = productList
					.ToList();
			}
			else
			{
				products = query
				.OrderByDescending(x => x.Id)
				.ToList();
			}

			//lọc theo orderby
			if (!string.IsNullOrEmpty(orderby))
			{
				if (orderby.Equals("price"))
				{
					products = products
						.OrderBy(x => x.SalePrice>0?x.SalePrice:x.Price)
						.ToList();
				}

                if (orderby.Equals("price-desc"))
                {
                    products = products
                        .OrderByDescending(x => x.SalePrice > 0 ? x.SalePrice : x.Price)
                        .ToList();
                }
            }

			//lọc theo giá
			if (minPrice.HasValue)
			{
				products = products
					.Where(x => (x.SalePrice > 0 ? x.SalePrice : x.Price) >= minPrice.Value)
					.ToList();
			}

			if (maxPrice.HasValue)
			{
				products = products
					.Where(x => (x.SalePrice > 0 ? x.SalePrice : x.Price) <= maxPrice.Value)
					.ToList();
			}

			// Tổng số sản phẩm sau khi lọc
			totalItems = totalItems > 0 ? totalItems : products.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
			decimal raw = _context.Products
									.Select(x => x.SalePrice > 0 ? x.SalePrice : x.Price)
									.Max() ?? 0m;

			// Làm tròn lên theo bậc 10.000 rồi convert sang long
			long topPrice = (long)(
				Math.Ceiling(raw / 10000m) * 10000m
			);

			products = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
				.ToList();

            // Lấy danh mục cha + con
            var categoryParent = _context.Categories.Where(x => x.ParentId == null).ToList();
			var categories = _context.Categories.Where(x => x.ParentId != null).ToList();

			ViewBag.Page = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.CategoryParent = categoryParent;
			ViewBag.Category = categories;
			ViewBag.ParentSlug = parentSlug;
			ViewBag.Slug = slug;
			ViewBag.Orderby = orderby;
			ViewBag.MinPrice = minPrice;
			ViewBag.MaxPrice = maxPrice;
			ViewBag.TopPrice = topPrice;
			return View(products);
		}


		[Route("chi-tiet/{slug}")]
		public IActionResult Details(string slug) 
		{
			var categories = _context.Categories
				.Include(c => c.Parent)
				.ToList();
			var product = _context.Products
				.Include(x => x.Category)
				.FirstOrDefault(x => x.Slug == slug);
			var productSpecs = _context.ProductSpecs
				.Where(x => x.ProductId == product.Id)
				.ToList();
			var productImages = _context.ProductImages
				.Where(x => x.ProductId == product.Id)
				.ToList();
			ViewBag.Categories = categories;
			ViewBag.ProductSpecs = productSpecs;
			ViewBag.ProductImages = productImages;
			return View("chi-tiet-san-pham", product);
		}
	}
}
