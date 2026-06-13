using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyBooker.Data;
using SkyBooker.Models;

namespace SkyBooker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost] // Creates data
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking) //creates a booking
        {
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.Id == booking.FlightId);

            if (flight == null) // checks for available seats on the flight and if the flight exists
            {
                return NotFound("Flight not found.");
            }

            if (flight.AvailableSeats <= 0)
            {
                return BadRequest("No available seats on this flight.");
            }

            flight.AvailableSeats--;

            _context.Bookings.Add(booking); //saves to SQL and returns the saved booking

            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();

            return Ok(bookings);
        }
    }
}