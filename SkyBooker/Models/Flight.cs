namespace SkyBooker.Models
{
    public class Flight
    {
        // Basically user Id, From, To, DepartureTime, price, AvailableSeats of where they are flying. 
        public int Id { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }


}
