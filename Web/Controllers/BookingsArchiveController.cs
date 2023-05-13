using DDPS.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDPS.Web.Controllers
{
    public class BookingsArchiveController : Controller
    {

        private readonly HotelContext _context;

        public BookingsArchiveController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.BookingsArchive.Include(a => a.Booking)
                .ThenInclude(a => a.Apartament).ThenInclude(f => f.Facilities)
                .Include(b => b.Booking).ThenInclude(a => a.Apartament).ThenInclude(s => s.Services)
                .Include(b => b.Booking).ThenInclude(c => c.Client)
                .Include(s => s.Booking).ThenInclude(s => s.Services)
                .Where(b => b.Booking.IsActive == false);

            return View(await context.ToListAsync());
        }
    }
}
