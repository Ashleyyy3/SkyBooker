namespace SkyBooker.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; } // a booking belongs to a flight 
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerEmail { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
