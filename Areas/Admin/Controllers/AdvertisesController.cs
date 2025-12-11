using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdvertisesController : Controller
    {
        private readonly DbMyShopContext _context;

        public AdvertisesController(DbMyShopContext context)
        {
            _context = context;
        }

        // GET: Admin/Advertises
        public async Task<IActionResult> Index(string? name ,int page = 1, int pageSize = 30)
        {
            var query = _context.Advertises.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim())).OrderByDescending(x=> x.Id);
            }
            // Tổng số bản ghi sau khi lọc
            var totalCount = await query.CountAsync();

            // Lấy dữ liệu từng trang
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi biến qua View
            ViewData["SearchName"] = name;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return View(data);
        }

        // GET: Admin/Advertises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertise = await _context.Advertises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertise == null)
            {
                return NotFound();
            }

            return View(advertise);
        }

        // GET: Admin/Advertises/Create
        public IActionResult Create()
        {
            List<Page> pages = _context.Pages.ToList();
            ViewBag.Page = pages;
            return View();
        }

        // POST: Admin/Advertises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advertise model)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0 && files[0].Length > 0)
            {
                var file = files[0];
                var FileName = file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    model.Image = FileName;
                }
            }
            _context.Advertises.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Advertises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertise = await _context.Advertises.FindAsync(id);
            if (advertise == null)
            {
                return NotFound();
            }
            return View(advertise);
        }

        // POST: Admin/Advertises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Width,Height,Link,Target,Content,Position,PageId,Click,Ord,Active,Lang")] Advertise advertise)
        {
            if (id != advertise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertiseExists(advertise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(advertise);
        }

        // GET: Admin/Advertises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertise = await _context.Advertises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertise == null)
            {
                return NotFound();
            }

            return View(advertise);
        }

        // POST: Admin/Advertises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertise = await _context.Advertises.FindAsync(id);
            if (advertise != null)
            {
                _context.Advertises.Remove(advertise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertiseExists(int id)
        {
            return _context.Advertises.Any(e => e.Id == id);
        }
    }
}
