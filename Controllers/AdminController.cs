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

        public IActionResult Bookings()
        {
            if (HttpContext.Session.GetString("AdminUser") == null)
                return RedirectToAction("Login");

            var bookings = _context.Bookings
                .Include(b => b.Route)
                .ThenInclude(r => r.Bus)
                .ToList();

            return View(bookings);
        }

        public IActionResult SearchBookings(string passengerName, string destination, DateTime? travelDate)
        {
            if (string.IsNullOrEmpty(passengerName) && string.IsNullOrEmpty(destination) && travelDate == null)
            {
                // No search yet, just show empty list
                return View(new List<Bookings>());
            }

            var bookings = _context.Bookings
                .Include(b => b.Route)
                .ThenInclude(r => r.Bus)
                .Where(b =>
                    (string.IsNullOrEmpty(passengerName) || b.PassengerName.Contains(passengerName)) &&
                    (string.IsNullOrEmpty(destination) || b.Route.Destination.Contains(destination)) &&
                    (travelDate == null || b.Route.DepartureTime.Date == travelDate.Value.Date)
                )
                .ToList();

            return View(bookings);
        }


        // Add Route

        public IActionResult AddRoute()
        {
            var buses = _context.Buses.ToList();
            ViewBag.Buses = buses;
            return View();
        }


        [HttpPost]
        public IActionResult AddRoute(Travel_Bud.Models.Route route) // Specify the correct Route type
        {
            _context.Routes.Add(route);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        public IActionResult EditRoute(int id)
        {
            var route = _context.Routes.Find(id);
            if (route == null) return NotFound();
            return View(route);
        }

        [HttpPost]
        public IActionResult EditRoute(Travel_Bud.Models.Route route)
        {
            _context.Routes.Update(route);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Delete Route
        public IActionResult DeleteRoute(int id)
        {
            var route = _context.Routes.Find(id);
            if (route == null) return NotFound();

            _context.Routes.Remove(route);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Edit Booking
        public IActionResult EditBooking(int id)
        {
            var booking = _context.Bookings.Include(b => b.Route).FirstOrDefault(b => b.BookingId == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        public IActionResult EditBooking(Bookings booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Delete Booking
        public IActionResult DeleteBooking(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



    }
}
