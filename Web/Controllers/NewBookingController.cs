using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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
            return View(await _context.Apartaments.Where(a => /*a.Reservation == false*/ a.Tariff.Id == (int)TempData.Peek("tariffId")).Include(t => t.Tariff).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> FourthStepApartament(int apartament)
        {
            /*TempData.Put<Apartaments>("apartament", apartament);*/
            TempData["apartamentId"] = apartament;
            return RedirectToAction(nameof(SixthStepServices));
        }
        #endregion

        #region SelectServices
        public async Task<IActionResult> SixthStepServices()
        {
            var apartId = (int)TempData.Peek("apartamentId");
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == (int)TempData.Peek("apartamentId")).First();

            /*var exceptServices = _context.Services.Except(inputApartament.Services).ToList();*/
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

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return RedirectToAction("Index", "Bookings");
        }
        #endregion
    }
}
