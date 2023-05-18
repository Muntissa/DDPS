using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using DDPS.Api.TempModels;
using Microsoft.AspNetCore.Identity;
using DDPS.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager, client")]
    public class ClientsController : Controller
    {
        private readonly HotelContext _context;
        private readonly UserManager<DDPSUser> _userManager;

        public ClientsController(HotelContext context, UserManager<DDPSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "manager"))
            {
                return _context.Clients != null ?
                              View(await _context.Clients.ToListAsync()) :
                              Problem("Entity set 'HotelContext.Clients'  is null.");
            }
            else
            {
                return View(await _context.Clients.Where(c => c.Email == user.Email).ToListAsync());
            }

        }
        [Authorize(Roles = "admin, manager, client")]
        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            ClientDetailsModel clientDetailsModel = new()
            {
                Client = clients,
                Bookings = _context.Bookings.Where(c => c.ClientId == id && c.IsActive).ToList(),
            };

            return View(clientDetailsModel);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SecondName,FirstName,LastName,Email,PhoneNumber")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.FindAsync(id);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }

        [Authorize(Roles = "admin, manager")]
        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SecondName,FirstName,LastName,Email,PhoneNumber")] Clients clients)
        {
            if (id != clients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
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
            return View(clients);
        }

        [Authorize(Roles = "admin")]
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        [Authorize(Roles = "admin")]
        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'HotelContext.Clients'  is null.");
            }
            var clients = await _context.Clients.FindAsync(id);
            if (clients != null)
            {
                _context.Clients.Remove(clients);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientsExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
