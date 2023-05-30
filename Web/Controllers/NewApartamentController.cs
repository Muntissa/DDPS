using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace DDPS.Web.Controllers
{
    public class NewApartamentController : Controller
    {
        private readonly HotelContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public NewApartamentController(HotelContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        #region SelectFirstProperies 
        [HttpGet]
        public async Task<IActionResult> FirstStep()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FirstStep(int apartamentNumber, int apartamentArea)
        {
            TempData["apartamentNumber"] = apartamentNumber;
            TempData["apartamentArea"] = apartamentArea;
            return RedirectToAction(nameof(SecondStepTariff));
        }
        #endregion

        #region SelectTariff 
        [HttpGet]
        public async Task<IActionResult> SecondStepTariff()
        {
            return View(await _context.Tariffs.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SecondStepTariff(int tariffId)
        {
            TempData["apartamentTariffId"] = tariffId;

            return RedirectToAction(nameof(ThirdStepFacilities));
        }
        #endregion

        #region SelectFacilites
        [HttpGet]
        public async Task<IActionResult> ThirdStepFacilities()
        {
            return View(await _context.Facilities.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> ThirdStepFacilities(List<int> facilitiesIds)
        {
            TempData["apartamentFacilities"] = facilitiesIds;

            return RedirectToAction(nameof(FourthStepServices));
        }
        #endregion

        #region SelectServices
        [HttpGet]
        public async Task<IActionResult> FourthStepServices()
        {
            return View(await _context.Services.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> FourthStepServices(List<int> servicesIds)
        {
            TempData["apartamentServices"] = servicesIds;

            return RedirectToAction(nameof(FifthStepPhoto));
        }
        #endregion

        #region SelectPhoto 
        [HttpGet]
        public async Task<IActionResult> FifthStepPhoto()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FifthStepPhoto(IFormFile apartamentPhoto)
        {
            string fileName = "";
            string filePath = "";

            if (apartamentPhoto != null && apartamentPhoto.Length > 0)
            {
                fileName = Path.GetFileName(apartamentPhoto.FileName);
                filePath = Path.Combine(_hostingEnvironment.WebRootPath, "files", fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await apartamentPhoto.CopyToAsync(fileStream);
                }
            }

            var servicesList = (int[])TempData.Peek("apartamentServices") ?? new int[0];
            var facilitiesList = (int[])TempData.Peek("apartamentFacilities") ?? new int[0];

            var newApartament = new Apartaments()
            {
                Number = (int)TempData.Peek("apartamentNumber"),
                Area = (int)TempData.Peek("apartamentArea"),
                Tariff = _context.Tariffs.Find((int)TempData.Peek("apartamentTariffId")),
                Services = _context.Services.Where(s => servicesList.Contains(s.Id)).ToList(),
                Facilities = _context.Facilities.Where(s => facilitiesList.Contains(s.Id)).ToList(),
                Photo = ("/files/" + fileName) == "/files/" ? ("/files/" + "NotFound.png") : ("/files/" + fileName)
            };

            _context.Apartaments.Add(newApartament);
            _context.SaveChanges();

            if ((bool?)TempData["FromNewBookingToApartaments"] == true)
            {
                return RedirectToAction("FourthStepApartament", "NewBooking");
            }

            return RedirectToAction("Index", "Apartaments");
        }
        #endregion

        #region RedirectToActions
        public IActionResult EditTariff(int tariffId)
        {
            return RedirectToAction("Edit", "Tariffs", new { id = tariffId });
        }

        public IActionResult CreateTariff()
        {
            return RedirectToAction("Create", "Tariffs", new { FromNewApartament = true });
        }

        public IActionResult EditFacility(int facilityId)
        {
            return RedirectToAction("Edit", "Facilities", new { id = facilityId });
        }

        public IActionResult CreateFacility()
        {
            return RedirectToAction("Create", "Facilities", new { FromNewApartament = true });
        }

        public IActionResult EditService(int serviceId)
        {
            return RedirectToAction("Edit", "Services", new { id = serviceId });
        }

        public IActionResult CreateService()
        {
            return RedirectToAction("Create", "Services", new { FromNewApartament = true });
        }
        #endregion
    }
}
