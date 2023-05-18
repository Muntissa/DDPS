using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;

namespace DDPS.Web.Controllers
{
    public class BookingsController : Controller
    {
        private readonly HotelContext _context;

        public BookingsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var context = _context.Bookings.Where(b => b.IsActive).Include(a => a.Apartament).ThenInclude(f => f.Facilities)
                .Include(a => a.Apartament).ThenInclude(s => s.Services)
                .Include(c => c.Client)
                .Include(s => s.Services);

            return View(await context.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Apartament)
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return RedirectToAction("FirstStepClient", "NewBooking");
        }

        public IActionResult GetServicesTabless()
        {
            return RedirectToAction("Index", "Services");
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Apartament)
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // POST: Bookings/Delete/5
        /*[Authorize(Roles = "test, test2")]*/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'HotelContext.Bookings'  is null.");
            }
            var bookings = await _context.Bookings.FindAsync(id);

            if (bookings != null)
            {
                bookings.Apartament.Reservation = false;
                bookings.IsActive = false;

                var inActiveBooking = new BookingsArchive()
                {
                    ApartamentNumber = bookings.Apartament.Id,
                    ApartamentAdditionalServices = new List<Services>(bookings.Services),
                    ClientSecondName = bookings.Client.SecondName,
                    ClientFirstName = bookings.Client.FirstName,
                    ClientLastName = bookings.Client.LastName,
                    BookingStartTime = bookings.StartTime,
                    BookingEndTime = bookings.EndTime,
                    InActiveTime = DateTime.Now,
                };

                _context.BookingsArchive.Add(inActiveBooking);
                _context.Bookings.Remove(bookings);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookingsExists(int id)
        {
            return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
