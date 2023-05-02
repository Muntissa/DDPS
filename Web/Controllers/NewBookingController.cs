using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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

        #region Select Apartament
        public async Task<IActionResult> FourthStepApartament()
        {
            return View(await _context.Apartaments.Where(a => /*a.Reservation == false*/ a.Tariff.Id == (int)TempData.Peek("tariffId")).Include(t => t.Tariff).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> FourthStepApartament(Apartaments apartament)
        {
            TempData["apartament"] = apartament;
            return RedirectToAction("SecondStepDate");
        }
        #endregion
    }
}
