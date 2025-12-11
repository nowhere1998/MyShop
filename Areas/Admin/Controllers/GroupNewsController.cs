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
    public class GroupNewsController : Controller
    {
        private readonly DbMyShopContext _context;

        public GroupNewsController(DbMyShopContext context)
        {
            _context = context;
        }

        // GET: Admin/GroupNews
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupNews.ToListAsync());
        }

        // GET: Admin/GroupNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupNews = await _context.GroupNews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupNews == null)
            {
                return NotFound();
            }

            return View(groupNews);
        }

        // GET: Admin/GroupNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/GroupNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Tag,Level,Title,Description,Keyword,Ord,Priority,Index,Active,Lang,Type1,Type2,Type3,Type4,Type5,Hinhanh")] GroupNews groupNews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupNews);
        }

        // GET: Admin/GroupNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupNews = await _context.GroupNews.FindAsync(id);
            if (groupNews == null)
            {
                return NotFound();
            }
            return View(groupNews);
        }

        // POST: Admin/GroupNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Tag,Level,Title,Description,Keyword,Ord,Priority,Index,Active,Lang,Type1,Type2,Type3,Type4,Type5,Hinhanh")] GroupNews groupNews)
        {
            if (id != groupNews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupNewsExists(groupNews.Id))
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
            return View(groupNews);
        }

        // GET: Admin/GroupNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupNews = await _context.GroupNews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupNews == null)
            {
                return NotFound();
            }

            return View(groupNews);
        }

        // POST: Admin/GroupNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupNews = await _context.GroupNews.FindAsync(id);
            if (groupNews != null)
            {
                _context.GroupNews.Remove(groupNews);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupNewsExists(int id)
        {
            return _context.GroupNews.Any(e => e.Id == id);
        }
    }
}
