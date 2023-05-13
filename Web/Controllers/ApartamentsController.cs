using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using System.Numerics;
using Microsoft.Extensions.Hosting;

namespace DDPS.Web.Controllers
{
    public class ApartamentsController : Controller
    {
        private readonly HotelContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ApartamentsController(HotelContext context, IWebHostEnvironment appEnviroment)
        {
            _context = context;
            _appEnvironment = appEnviroment;
        }

        public IActionResult GetImage(int id)
        {
            var entity = _context.Apartaments.FirstOrDefault(e => e.Id == id);

            if (entity != null && !string.IsNullOrEmpty(entity.Photo))
            {
                var imagePath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/", entity.Photo);
                /*                var imageFileStream = System.IO.File.OpenRead(imagePath);*/
                /*                return File(imageFileStream, "image/jpeg");*/
            }

            return NotFound();
        }

        // GET: Apartaments
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Apartaments.Include(a => a.Tariff);
            LoadPhotoBytesToViewBag(hotelContext);
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
            return RedirectToAction("FirstStep", "NewApartament");
        }

        public IActionResult GetTariffTables()
        {
            return RedirectToAction("Index", "Tariffs");
        }

        // POST: Apartaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Area,Price,TariffId, Photo")] Apartaments apartaments, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    apartaments.Photo = path;
                }

                _context.Add(apartaments);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Description", apartaments.TariffId);
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
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Description", apartaments.TariffId);
            LoadPhotoBytesToViewBag(apartaments);
            return View(apartaments);
        }

        // POST: Apartaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,ImageUrl,Area,Price,TariffId")] Apartaments apartaments, IFormFile upload)
        {
            if (id != apartaments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new
                   FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    if (!String.IsNullOrEmpty(apartaments.Photo))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath +
                       apartaments.Photo);
                    }
                    apartaments.Photo = path;
                }

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
            ViewData["TariffId"] = new SelectList(_context.Tariffs, "Id", "Description", apartaments.TariffId);
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
                var booking = _context.Bookings.Where(a => a.ApartamentId == id).FirstOrDefault();

                if (booking != null)
                    _context.Bookings.Remove(booking);

                _context.Apartaments.Remove(apartaments);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFacility(int facilityId, int apartamentId)
        {
            var facilityToDelete = _context.Facilities.Find(facilityId);
            var apartament = _context.Apartaments.Find(apartamentId);

            apartament.Facilities.Remove(facilityToDelete);

            _context.SaveChanges();

            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        [HttpGet]
        public async Task<IActionResult> AddFacilities(int apartamentId)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            return View(await _context.Facilities.Where(f => !inputApartament.Facilities.Contains(f)).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddFacilities(int apartamentId, List<int> facilitiesIds)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            inputApartament.Facilities.AddRange(_context.Facilities.Where(s => facilitiesIds.Contains(s.Id)).ToList());

            _context.SaveChanges();
            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        public IActionResult DeleteService(int serviceId, int apartamentId)
        {
            var serviceToDelete = _context.Services.Find(serviceId);
            var apartament = _context.Apartaments.Find(apartamentId);

            apartament.Services.Remove(serviceToDelete);

            _context.SaveChanges();

            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        [HttpGet]
        public async Task<IActionResult> AddServices(int apartamentId)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            return View(await _context.Services.Where(f => !inputApartament.Services.Contains(f)).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddServices(int apartamentId, List<int> servicesIds)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            inputApartament.Services.AddRange(_context.Services.Where(s => servicesIds.Contains(s.Id)).ToList());

            _context.SaveChanges();
            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }


        private bool ApartamentsExists(int id)
        {
            return (_context.Apartaments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void LoadPhotoBytesToViewBag(IEnumerable<Apartaments>? apartaments)
        {
            List<byte[]> bytes = new List<byte[]>();

            foreach (var apart in apartaments)
            {
                if (apart != null)
                {
                    if (!String.IsNullOrEmpty(apart.Photo))
                    {
                        byte[] photodata = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + apart.Photo);
                        bytes.Add(photodata);
                    }
                }
                else
                {
                    bytes.Add(null);
                }
            }
            ViewBag.PhotoBytes = bytes;
        }

        private void LoadPhotoBytesToViewBag(Apartaments? apartaments)
        {
            if (apartaments != null)
            {
                if (!String.IsNullOrEmpty(apartaments.Photo))
                {
                    byte[] photodata = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + apartaments.Photo);

                    ViewBag.Photodata = photodata;
                }
            }
            else
            {
                ViewBag.Photodata = null;
            }
        }
    }
}
