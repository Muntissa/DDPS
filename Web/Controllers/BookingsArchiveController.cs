using DDPS.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class BookingsArchiveController : Controller
    {

        private readonly HotelContext _context;

        public BookingsArchiveController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.BookingsArchive;

            return View(await context.ToListAsync());
        }
    }
}
