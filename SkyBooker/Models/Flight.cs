namespace SkyBooker.Models
{
    public class Flight
    {
        // Basically user Id, From, To, DepartureTime, price, AvailableSeats of where they are flying. 
        public int Id { get; set; }

        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;

        public string Airline { get; set; } = string.Empty;
        public string AirlineCode { get; set; } = string.Empty;

        public string DepartureAirportCode { get; set; } = string.Empty;
        public string ArrivalAirportCode { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }
        public int Stops { get; set; }

        public string Provider { get; set; } = string.Empty;
        public string BookingUrl { get; set; } = string.Empty;

        public int AvailableSeats { get; set; }
    }

  


    }
