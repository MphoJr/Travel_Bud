using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_Bud.Data;
using Travel_Bud.Models; // Add this using directive

namespace Travel_Bud.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Login form
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminUser", admin.Username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminUser") == null)
                return RedirectToAction("Login");

            var bookings = _context.Bookings.Include(b => b.Route).ThenInclude(r => r.Bus).ToList();
            var routes = _context.Routes.Include(r => r.Bus).ToList();

            ViewBag.Routes = routes;
            return View(bookings);
        }

        // Add Route
        [HttpPost]
        public IActionResult AddRoute(Travel_Bud.Models.Route route) // Specify the correct Route type
        {
            _context.Routes.Add(route);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
