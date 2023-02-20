using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;

namespace DDPS.Web.Controllers
{
    public class ApartamentsController : Controller
    {
        private readonly HotelContext _context;

        public ApartamentsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Apartaments
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Apartaments.Include(a => a.Tariff);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Apartaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Apartaments == null)
            {
                return NotFound();
            }

            var apartaments = await _context.Apartaments
                .Include(a => a.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartaments == null)
            {
                return NotFound();
            }

            return View(apartaments);
        }

        // GET: Apartaments/Create
        public IActionResult Create()
        {
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Name");
            return View();
        }

        // POST: Apartaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,ImageUrl,Area,Price,TariffId")] Apartaments apartaments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apartaments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Name", apartaments.TariffId);
            return View(apartaments);
        }

        // GET: Apartaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Apartaments == null)
            {
                return NotFound();
            }

            var apartaments = await _context.Apartaments.FindAsync(id);
            if (apartaments == null)
            {
                return NotFound();
            }
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Name", apartaments.TariffId);
            return View(apartaments);
        }

        // POST: Apartaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,ImageUrl,Area,Price,TariffId")] Apartaments apartaments)
        {
            if (id != apartaments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartaments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartamentsExists(apartaments.Id))
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
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Name", apartaments.TariffId);
            return View(apartaments);
        }

        // GET: Apartaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Apartaments == null)
            {
                return NotFound();
            }

            var apartaments = await _context.Apartaments
                .Include(a => a.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartaments == null)
            {
                return NotFound();
            }

            return View(apartaments);
        }

        // POST: Apartaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Apartaments == null)
            {
                return Problem("Entity set 'HotelContext.Apartaments'  is null.");
            }
            var apartaments = await _context.Apartaments.FindAsync(id);
            if (apartaments != null)
            {
                _context.Apartaments.Remove(apartaments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartamentsExists(int id)
        {
          return (_context.Apartaments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
