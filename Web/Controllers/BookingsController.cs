using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using DDPS.Web.Areas.Identity.Data;
using OfficeOpenXml;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager, client")]
    public class BookingsController : Controller
    {
        private readonly HotelContext _context;
        private readonly UserManager<DDPSUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public BookingsController(HotelContext context, UserManager<DDPSUser> userManager, IWebHostEnvironment appEnviroment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnviroment;
        }


        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "manager"))
            {
                var context = _context.Bookings.Where(b => b.IsActive)
                    .Include(a => a.Apartament).ThenInclude(f => f.Facilities)
                    .Include(a => a.Apartament).ThenInclude(s => s.Services)
                    .Include(c => c.Client)
                    .Include(s => s.Services);

                return View(await context.ToListAsync());
            }
            else
            {
                return View(await _context.Bookings.Where(b => b.Client.Email == user.Email && b.IsActive)
                    .Include(a => a.Apartament).ThenInclude(f => f.Facilities)
                    .Include(a => a.Apartament).ThenInclude(s => s.Services)
                    .Include(c => c.Client)
                    .Include(s => s.Services).ToListAsync());
            }
        }

        [Authorize(Roles = "admin, manager, client")]
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

        [Authorize(Roles = "admin, manager")]
        // GET: Bookings/Create
        public IActionResult Create()
        {
            return RedirectToAction("FirstStepClient", "NewBooking");
        }

        [Authorize(Roles = "admin, manager")]
        public IActionResult GetServicesTabless()
        {
            return RedirectToAction("Index", "Services");
        }

        [Authorize(Roles = "admin, manager")]
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

        [Authorize(Roles = "admin, manager")]
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

        [Authorize(Roles = "admin, manager")]
        public FileResult GetAllBookingsReport()
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/CurrentOrdersTemplate.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/CurrentOrders.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Андронов И.А.";
                excelPackage.Workbook.Properties.Title = "Список всех действующих заказов";
                excelPackage.Workbook.Properties.Subject = "Действующие заказы";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Bookings"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int startLine = 2;

                List<Bookings> bookings = _context.Bookings
                    .Include(a => a.Apartament).ThenInclude(f => f.Facilities)
                    .Include(a => a.Apartament).ThenInclude(s => s.Services)
                    .Include(c => c.Client)
                    .Include(s => s.Services).ToList();

                foreach (var booking in bookings)
                {
                    worksheet.Cells[startLine, 1].Value = booking.Apartament.Number;
                    worksheet.Cells[startLine, 2].Value = booking.Client.SecondName + booking.Client.FirstName + booking.Client.LastName;
                    worksheet.Cells[startLine, 3].Value = booking.Apartament.Tariff.Name;
                    worksheet.Cells[startLine, 4].Value = booking.StartTime.ToString("dd-MM-yyyy");
                    worksheet.Cells[startLine, 5].Value = booking.EndTime.ToString("dd-MM-yyyy");
                    worksheet.Cells[startLine, 6].Value = String.Join(Environment.NewLine, booking.Services.Select(s => s.Name));
                    worksheet.Cells[startLine, 7].Value = String.Join(Environment.NewLine, booking.Apartament.Services.Select(s => s.Name));
                    worksheet.Cells[startLine, 8].Value = (booking.Services.Sum(s => s.Price) + (booking.EndTime - booking.StartTime).Days * (booking.Apartament.Tariff.Price + (booking.Apartament.Area * 50) + (int)(0.5 * booking.Apartament.Services.Sum(s => s.Price))));

                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type =
           "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = "AllBookings.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
