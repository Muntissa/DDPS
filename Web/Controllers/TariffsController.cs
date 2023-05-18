using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager, client")]
    public class TariffsController : Controller
    {
        private readonly HotelContext _context;

        public TariffsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Tariffs
        public async Task<IActionResult> Index()
        {
              return _context.Tariffs != null ? 
                          View(await _context.Tariffs.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Tariffs'  is null.");
        }

        [Authorize(Roles = "admin, manager, client")]
        // GET: Tariffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tariffs == null)
            {
                return NotFound();
            }

            var tariffs = await _context.Tariffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariffs == null)
            {
                return NotFound();
            }

            return View(tariffs);
        }

        [Authorize(Roles = "admin")]
        // GET: Tariffs/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: Tariffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Tariffs tariffs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tariffs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tariffs);
        }

        [Authorize(Roles = "admin")]
        // GET: Tariffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tariffs == null)
            {
                return NotFound();
            }

            var tariffs = await _context.Tariffs.FindAsync(id);
            if (tariffs == null)
            {
                return NotFound();
            }
            return View(tariffs);
        }

        [Authorize(Roles = "admin")]
        // POST: Tariffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Tariffs tariffs)
        {
            if (id != tariffs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tariffs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TariffsExists(tariffs.Id))
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
            return View(tariffs);
        }

        [Authorize(Roles = "admin")]
        // GET: Tariffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tariffs == null)
            {
                return NotFound();
            }

            var tariffs = await _context.Tariffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariffs == null)
            {
                return NotFound();
            }

            return View(tariffs);
        }

        [Authorize(Roles = "admin")]
        // POST: Tariffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tariffs == null)
            {
                return Problem("Entity set 'HotelContext.Tariffs'  is null.");
            }
            var tariffs = await _context.Tariffs.FindAsync(id);
            if (tariffs != null)
            {
                _context.Tariffs.Remove(tariffs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TariffsExists(int id)
        {
          return (_context.Tariffs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
