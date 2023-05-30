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
    public class FacilitiesController : Controller
    {
        private readonly HotelContext _context;

        public FacilitiesController(HotelContext context)
        {
            _context = context;
        }

        // GET: Facilities
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
              return _context.Facilities != null ? 
                          View(await _context.Facilities.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Facilities'  is null.");
        }

        [Authorize(Roles = "admin")]
        // GET: Facilities/Create
        public IActionResult Create(bool? FromNewApartament)
        {
            if (FromNewApartament.HasValue && FromNewApartament.Value is true)
            {
                TempData["FromNewApartamentToFacilities"] = FromNewApartament.Value;
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Facilities facilities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facilities);
                await _context.SaveChangesAsync();
                if ((bool?)TempData["FromNewApartamentToFacilities"] == true)
                {
                    return RedirectToAction("ThirdStepFacilities", "NewApartament");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(facilities);
        }

        [Authorize(Roles = "admin")]
        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities == null)
            {
                return NotFound();
            }
            return View(facilities);
        }

        [Authorize(Roles = "admin")]
        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Facilities facilities)
        {
            if (id != facilities.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facilities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilitiesExists(facilities.Id))
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
            return View(facilities);
        }

        [Authorize(Roles = "admin")]
        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facilities = await _context.Facilities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facilities == null)
            {
                return NotFound();
            }

            return View(facilities);
        }

        [Authorize(Roles = "admin")]
        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'HotelContext.Facilities'  is null.");
            }
            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities != null)
            {
                _context.Facilities.Remove(facilities);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilitiesExists(int id)
        {
          return (_context.Facilities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
