using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Net;
using System.Reflection;

namespace DDPS.Web.Controllers
{
    public class NewBookingController : Controller
    {

        private readonly HotelContext _context;

        public NewBookingController(HotelContext context)
        {
            _context = context;
        }
        #region SelectClients 
        [HttpGet]
        public async Task<IActionResult> FirstStepClient()
        {
            return View(await _context.Clients.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> FirstStepClient(int clientId)
        {
            TempData["clientId"] = clientId;
            return RedirectToAction(nameof(SecondStepDate));
        }
        #endregion

        #region SelectDate
        public async Task<IActionResult> SecondStepDate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SecondStepDate(DateTime startDate, DateTime endDate)
        {
            TempData["startDate"] = startDate;
            TempData["endDate"] = endDate;

            return RedirectToAction(nameof(ThirdStepTariff));
        }
        #endregion

        #region SelectTariff
        public async Task<IActionResult> ThirdStepTariff()
        {
            return View(await _context.Tariffs.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> ThirdStepTariff(int tariffId)
        {
            TempData["tariffId"] = tariffId;
            return RedirectToAction("FourthStepApartament");
        }
        #endregion

        #region SelectApartament
        public async Task<IActionResult> FourthStepApartament()
        {

            DateTime s = (DateTime)TempData.Peek("startDate");
            DateTime e = (DateTime)TempData.Peek("endDate");
            int t = (int)TempData.Peek("tariffId");

            List<Apartaments> apartaments = new List<Apartaments>();

            foreach (Apartaments app in _context.Apartaments.Where(app => app.TariffId == t))
            {
                if(!_context.Bookings.Any(b => b.ApartamentId == app.Id && 
                    ((b.StartTime >= s && b.EndTime <= s) || (b.StartTime >= e && b.EndTime <= e))))
                {
                    apartaments.Add(app);
                }
            }

            var additionalApartaments = _context.Apartaments.ToList();

            apartaments.AddRange(_context.Set<Apartaments>().Where(a => a.TariffId == (int)TempData.Peek("tariffId") && !apartaments.Contains(a)));

            return View(apartaments);
        }
        [HttpPost]
        public async Task<IActionResult> FourthStepApartament(int apartament)
        {
            TempData["apartamentId"] = apartament;
            return RedirectToAction(nameof(SixthStepServices));
        }
        #endregion

        #region SelectServices
        public async Task<IActionResult> SixthStepServices()
        {
            var apartId = (int)TempData.Peek("apartamentId");

            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == (int)TempData.Peek("apartamentId")).First();

            return View(await _context.Services.Where(s => !inputApartament.Services.Contains(s)).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SixthStepServices(List<int> servicesIds)
        {
            var services = servicesIds;

            var newBooking = new Bookings()
            {
                Apartament = _context.Apartaments.Where(a => a.Id == (int)TempData.Peek("apartamentId")).First(),
                Client = _context.Clients.Where(c => c.Id == (int)TempData.Peek("clientId")).First(),
                StartTime = (DateTime)TempData.Peek("startDate"),
                EndTime = (DateTime)TempData.Peek("endDate"),
                Services = _context.Services.Where(s => servicesIds.Contains(s.Id)).ToList(),
            };

            _context.Apartaments.Where(a => a.Id == (int)TempData.Peek("apartamentId")).First().Reservation = true;

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return RedirectToAction("Index", "Bookings");
        }
        #endregion

        #region RedirectToActions
        public IActionResult EditClient(int clientId)
        {
            return RedirectToAction("Edit", "Clients", new { id = clientId });
        }

        public IActionResult CreateClient()
        {
            return RedirectToAction("Create", "Clients");
        }

        public IActionResult EditTariff(int tariffId)
        {
            return RedirectToAction("Edit", "Tariffs", new { id = tariffId });
        }

        public IActionResult CreateTariff()
        {
            return RedirectToAction("Create", "Tariffs");
        }

        public IActionResult EditApartament(int apartamentId)
        {
            return RedirectToAction("Edit", "Tariffs", new { id = apartamentId });
        }

        public IActionResult CreateApartament()
        {
            return RedirectToAction("Create", "Tariffs");
        }

        public IActionResult EditService(int serviceId)
        {
            return RedirectToAction("Edit", "Services", new { id = serviceId });
        }

        public IActionResult CreateService()
        {
            return RedirectToAction("Create", "Services");
        }
        #endregion
    }
}
