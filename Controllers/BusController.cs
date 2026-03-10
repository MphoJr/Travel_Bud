using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel_Bud.Data;
using Travel_Bud.Models;

namespace Travel_Bud.Controllers
{
    public class BusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    
        private readonly ApplicationDbContext _context;

        public BusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Search form
        public IActionResult Search()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Search(string startLocation, string destination, DateTime? travelDate)
        {
            if (string.IsNullOrEmpty(startLocation) || string.IsNullOrEmpty(destination) || travelDate == null)
            {
                // No search yet, just show empty list
                return View(new List<Travel_Bud.Models.Route>());
            }

            var buses = _context.Routes
                .Where(r => r.StartLocation == startLocation
                         && r.Destination == destination
                         && travelDate.Value.Date == travelDate.Value.Date) // match date (see explanation)
                .Include(r => r.Bus)
                .ToList();

            return View(buses);
        }




        // GET: Results after search
        [HttpGet]
        public IActionResult Results(string startLocation, string destination)
        {
            var buses = _context.Routes
                .Where(r => r.StartLocation == startLocation && r.Destination == destination)
                .Include(r => r.Bus)
                .ToList();

            return View(buses); // goes to Results.cshtml
        }

        // GET: Book a bus
        public IActionResult Book(int id)
        {
            var route = _context.Routes
                .Include(r => r.Bus)
                .FirstOrDefault(r => r.RouteId == id);

            if (route == null)
                return NotFound();

            return View(route); // goes to Book.cshtml
        }

        // POST: Confirm booking
        [HttpPost]
        public IActionResult ConfirmBooking(int routeId, string passengerName)
        {
            var booking = new Bookings
            {
                RouteId = routeId,
                PassengerName = passengerName
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Success");
        }

        // GET: Success page
        public IActionResult Success()
        {
            return View(); // goes to Success.cshtml
        }
    }
}

