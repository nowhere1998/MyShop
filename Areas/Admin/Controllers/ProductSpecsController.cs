using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductSpecsController : Controller
    {
        private readonly DbMyShopContext _context;

        public ProductSpecsController(DbMyShopContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductSpecs
        public async Task<IActionResult> Index()
        {
            var dbMyShopContext = _context.ProductSpecs.Include(p => p.Product);
            return View(await dbMyShopContext.ToListAsync());
        }

        // GET: Admin/ProductSpecs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSpec = await _context.ProductSpecs
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSpec == null)
            {
                return NotFound();
            }

            return View(productSpec);
        }

        // GET: Admin/ProductSpecs/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Admin/ProductSpecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,SpecName,SpecValue")] ProductSpec productSpec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSpec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productSpec.ProductId);
            return View(productSpec);
        }

        // GET: Admin/ProductSpecs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSpec = await _context.ProductSpecs.FindAsync(id);
            if (productSpec == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productSpec.ProductId);
            return View(productSpec);
        }

        // POST: Admin/ProductSpecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ProductId,SpecName,SpecValue")] ProductSpec productSpec)
        {
            if (id != productSpec.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSpec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSpecExists(productSpec.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productSpec.ProductId);
            return View(productSpec);
        }

        // GET: Admin/ProductSpecs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSpec = await _context.ProductSpecs
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSpec == null)
            {
                return NotFound();
            }

            return View(productSpec);
        }

        // POST: Admin/ProductSpecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productSpec = await _context.ProductSpecs.FindAsync(id);
            if (productSpec != null)
            {
                _context.ProductSpecs.Remove(productSpec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSpecExists(long id)
        {
            return _context.ProductSpecs.Any(e => e.Id == id);
        }
    }
}
