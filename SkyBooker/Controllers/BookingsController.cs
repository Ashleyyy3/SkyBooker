using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyBooker.Data;
using SkyBooker.DTOs;
using SkyBooker.Models;
using SkyBooker.DTOs;

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
        public async Task<ActionResult<BookingResponseDto>> CreateBooking(CreateBookingDto dto)
        {
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.Id == dto.FlightId);

            if (flight == null)
            {
                return NotFound("Flight not found.");
            }

            if (flight.AvailableSeats <= 0)
            {
                return BadRequest("No available seats on this flight.");
            }

            var booking = new Booking
            {
                FlightId = dto.FlightId,
                PassengerName = dto.PassengerName,
                PassengerEmail = dto.PassengerEmail,
                BookingDate = DateTime.Now
            };

            flight.AvailableSeats--;

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            var response = new BookingResponseDto
            {
                Id = booking.Id,
                FlightId = booking.FlightId,
                PassengerName = booking.PassengerName,
                PassengerEmail = booking.PassengerEmail,
                BookingDate = booking.BookingDate
            };

            return Ok(response);
        }
    }
}