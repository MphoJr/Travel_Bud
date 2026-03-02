namespace Travel_Bud.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string StartLocation { get; set; }
        public string Destination { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public decimal Price { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }



    }
}
