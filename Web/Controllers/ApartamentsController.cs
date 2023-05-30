using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Identity;
using DDPS.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager, client")]
    public class ApartamentsController : Controller
    {
        private readonly HotelContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<DDPSUser> _userManager;

        public ApartamentsController(HotelContext context, IWebHostEnvironment appEnviroment, UserManager<DDPSUser> userManager)
        {
            _context = context;
            _appEnvironment = appEnviroment;
            _userManager = userManager;
        }

        // GET: Apartaments
        public async Task<IActionResult> Index()
        {
            TempData.Clear();

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "manager"))
            {
                var hotelContext = _context.Apartaments.Include(a => a.Tariff);
                return View(await hotelContext.ToListAsync());
            }
            else
            {
                return View(await _context.Bookings.Where(b => b.Client.Email == user.Email).Select(b => b.Apartament).ToListAsync());
            }
            
        }

        [Authorize(Roles = "admin, manager, client")]
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

        [Authorize(Roles = "admin")]
        // GET: Apartaments/Create
        public IActionResult Create(bool? FromNewBooking)
        {
            if (FromNewBooking.HasValue && FromNewBooking.Value is true)
            {
                TempData["FromNewBookingToApartaments"] = FromNewBooking.Value;
            }
            return RedirectToAction("FirstStep", "NewApartament");
        }

        [Authorize(Roles = "admin, manager, client")]
        public IActionResult GetTariffTables()
        {
            return RedirectToAction("Index", "Tariffs");
        }

        [Authorize(Roles = "admin, manager")]
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
            return View(apartaments);
        }

        [Authorize(Roles = "admin, manager")]
        // POST: Apartaments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,ImageUrl,Area,Price,TariffId")] Apartaments apartaments, IFormFile upload)
        {
            if (id != apartaments.Id)
            {
                return NotFound();
            }

            var fileName = "";
            var filePath = "";

            if (upload != null && upload.Length > 0)
            {
                fileName = Path.GetFileName(upload.FileName);
                filePath = Path.Combine(_appEnvironment.WebRootPath, "files", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
                apartaments.Photo = "/files/" + fileName;
            }

            if (ModelState.IsValid)
            {   
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
                _context.Bookings.RemoveRange(_context.Bookings.Where(a => a.ApartamentId == apartaments.Id));

                _context.Apartaments.Remove(apartaments);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteFacility(int facilityId, int apartamentId)
        {
            var facilityToDelete = _context.Facilities.Find(facilityId);
            var apartament = _context.Apartaments.Find(apartamentId);

            apartament.Facilities.Remove(facilityToDelete);

            _context.SaveChanges();

            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        [Authorize(Roles = "admin, manager")]
        [HttpGet]
        public async Task<IActionResult> AddFacilities(int apartamentId)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            return View(await _context.Facilities.Where(f => !inputApartament.Facilities.Contains(f)).ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        public async Task<IActionResult> AddFacilities(int apartamentId, List<int> facilitiesIds)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            inputApartament.Facilities.AddRange(_context.Facilities.Where(s => facilitiesIds.Contains(s.Id)).ToList());

            _context.SaveChanges();
            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteService(int serviceId, int apartamentId)
        {
            var serviceToDelete = _context.Services.Find(serviceId);
            var apartament = _context.Apartaments.Find(apartamentId);

            apartament.Services.Remove(serviceToDelete);

            _context.SaveChanges();

            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }

        [Authorize(Roles = "admin, manager")]
        [HttpGet]
        public async Task<IActionResult> AddServices(int apartamentId)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            return View(await _context.Services.Where(f => !inputApartament.Services.Contains(f)).ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        public async Task<IActionResult> AddServices(int apartamentId, List<int> servicesIds)
        {
            var inputApartament = _context.Set<Apartaments>().Where(a => a.Id == apartamentId).First();

            inputApartament.Services.AddRange(_context.Services.Where(s => servicesIds.Contains(s.Id)).ToList());

            _context.SaveChanges();
            return RedirectToAction("Edit", "Apartaments", new { id = apartamentId });
        }


        [Authorize(Roles = "admin, manager")]
        public FileResult GetAvaibleApartamentsReport(DateTime startDate, DateTime endDate)
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/ApartamentsByDateTemplate.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/ApartamentsByDate.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Андронов И.А.";
                excelPackage.Workbook.Properties.Title = $"Список апартаментов, свободных в даты {startDate.Date} - {endDate.Date}";
                excelPackage.Workbook.Properties.Subject = $"Апартаменты, свободные в даты {startDate.Date} - {endDate.Date}";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["ApartamentsByDate"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int startLine = 2;

                List<Apartaments> apartaments = new List<Apartaments>();

                foreach (Apartaments app in _context.Apartaments.ToList())
                {
                    if (!_context.Bookings.Any(b => b.ApartamentId == app.Id &&
                                    ((b.StartTime >= startDate && b.StartTime < endDate) ||
                                     (b.EndTime > startDate && b.EndTime <= endDate) ||
                                     (b.StartTime <= startDate && b.EndTime >= endDate))))
                    {
                        apartaments.Add(app);
                    }
                }

                /* apartaments.AddRange(_context.Set<Apartaments>().Where(a => !apartaments.Contains(a)));*/

                foreach (var apartament in apartaments)
                {
                    worksheet.Cells[startLine, 1].Value = apartament.Number;
                    worksheet.Cells[startLine, 2].Value = apartament.Area;
                    worksheet.Cells[startLine, 3].Value = apartament.Tariff.Price + (apartament.Area * 50) + (int)(0.5 * apartament.Services.Sum(s => s.Price));
                    worksheet.Cells[startLine, 4].Value = apartament.Tariff.Name;
                    worksheet.Cells[startLine, 5].Value = String.Join(Environment.NewLine, apartament.Services.Select(s => s.Name));

                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type =
           "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = $"Список апартаментов, свободных в даты {startDate.ToShortDateString()} - {endDate.Date.ToShortDateString()}.xlsx";
            return File(result, file_type, file_name);
        }

        private bool ApartamentsExists(int id)
        {
            return (_context.Apartaments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
