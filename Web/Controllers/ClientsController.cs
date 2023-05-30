using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDPS.Api;
using DDPS.Api.Entities;
using DDPS.Api.TempModels;
using Microsoft.AspNetCore.Identity;
using DDPS.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OfficeOpenXml;

namespace DDPS.Web.Controllers
{
    [Authorize(Roles = "admin, manager, client")]
    public class ClientsController : Controller
    {
        private readonly HotelContext _context;
        private readonly UserManager<DDPSUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public ClientsController(HotelContext context, UserManager<DDPSUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }


        // GET: Clients
        public async Task<IActionResult> Index()
        {
            TempData.Clear();
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
        public IActionResult Create(bool? FromNewBooking)
        {
            if(FromNewBooking.HasValue && FromNewBooking.Value is true)
            {
                TempData["FromNewBookingToClients"] = FromNewBooking.Value;
            }
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
                if ((bool?)TempData["FromNewBookingToClients"] == true)
                {
                    return RedirectToAction("FirstStepClient", "NewBooking");
                }
                
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
                foreach(var booking in _context.Bookings.Where(b => b.Client.Id == clients.Id).ToList())
                {
                    _context.Bookings.Remove(booking);
                }
                _context.Clients.Remove(clients);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<FileResult> GetBookingsForCurrentClient(int? id)
        {
            // Путь к файлу с шаблоном
            string path = "/Reports/OrdersForCurrentClientTemplate.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/OrdersForCurrentClient.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //открываем файл с шаблоном

            var client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //устанавливаем поля документа
                excelPackage.Workbook.Properties.Author = "Андронов И.А.";
                excelPackage.Workbook.Properties.Title = $"Список всех заказов клиента {client.SecondName} {client.FirstName} {client.LastName}";
                excelPackage.Workbook.Properties.Subject = $"Заказы клиента {client.SecondName} {client.FirstName} {client.LastName}";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["ClientsOrders"];
                //получаем списко пользователей и в цикле заполняем лист данными
                int startLine = 2;

                var bookings = _context.Bookings.Where(c => c.ClientId == id && c.IsActive)
                    .Include(a => a.Apartament).ThenInclude(f => f.Facilities)
                    .Include(a => a.Apartament).ThenInclude(s => s.Services)
                    .Include(c => c.Client)
                    .Include(s => s.Services).ToList();

                foreach (var booking in bookings)
                {
                    worksheet.Cells[startLine, 1].Value = booking.Apartament.Number;
                    worksheet.Cells[startLine, 2].Value = booking.Apartament.Tariff.Name;
                    worksheet.Cells[startLine, 3].Value = booking.StartTime.ToString("dd-MM-yyyy");
                    worksheet.Cells[startLine, 4].Value = booking.EndTime.ToString("dd-MM-yyyy");
                    worksheet.Cells[startLine, 5].Value = String.Join(Environment.NewLine, booking.Services.Select(s => s.Name));
                    worksheet.Cells[startLine, 6].Value = String.Join(Environment.NewLine, booking.Apartament.Services.Select(s => s.Name));
                    worksheet.Cells[startLine, 7].Value = (booking.Services.Sum(s => s.Price) + (booking.EndTime - booking.StartTime).Days * (booking.Apartament.Tariff.Price + (booking.Apartament.Area * 50) + (int)(0.5 * booking.Apartament.Services.Sum(s => s.Price))));

                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = $"{client.SecondName} {client.FirstName[0]}. {client.LastName[0]}. Bookings.xlsx";
            
            return File(result, file_type, file_name);
        }

        private bool ClientsExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
