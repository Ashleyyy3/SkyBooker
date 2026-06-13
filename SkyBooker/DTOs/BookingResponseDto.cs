namespace SkyBooker.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }

        public int FlightId { get; set; }

        public string PassengerName { get; set; } = string.Empty;

        public string PassengerEmail { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }
    }
}