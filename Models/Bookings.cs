namespace Travel_Bud.Models
{
    public class Bookings
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }



    }
}
