using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.Models;


namespace SkyBooker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        [HttpGet] // Reads data
        public ActionResult<List<Flight>> GetFlights()
        {
            var flights = new List<Flight>
            {
                new Flight
                {
                    Id = 1,
                    From = "Stockholm",
                    To = "London",
                    DepartureTime = new DateTime(2026, 7, 1, 8, 30, 0),
                    Price = 1299,
                    AvailableSeats = 50
                },
                new Flight
                {
                    Id = 2,
                    From = "Gothenburg",
                    To = "Paris",
                    DepartureTime = new DateTime(2026, 7, 2, 10, 0, 0),
                    Price = 1599,
                    AvailableSeats = 35
                },
                 new Flight
                {
                    Id = 3,
                    From = "Paris",
                    To = "London",
                    DepartureTime = new DateTime(2026, 7, 3, 20, 0, 0),
                    Price = 1399,
                    AvailableSeats = 40
                }
            };

            return Ok(flights);
        }
    }
}
