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

        //Shows all bookings for a specific email address
        [HttpGet("by-email")]
        public async Task<ActionResult<List<BookingResponseDto>>> GetBookingsByEmail(string email)
        {
            var bookings = await _context.Bookings
                .Where(b => b.PassengerEmail.ToLower() == email.ToLower())
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    FlightId = b.FlightId,
                    PassengerName = b.PassengerName,
                    PassengerEmail = b.PassengerEmail,
                    BookingDate = b.BookingDate
                })
                .ToListAsync();

            return Ok(bookings);
        }

        //Cancel bookings

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.Id == booking.FlightId);

            if (flight != null)
            {
                flight.AvailableSeats++;
            }

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();

            return Ok("Booking cancelled successfully.");
        }
    }
}