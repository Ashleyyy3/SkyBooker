using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyBooker.Data;
using SkyBooker.Models;

namespace SkyBooker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Flight>>> GetFlights()
        {
            var flights = await _context.Flights.ToListAsync(); //gives us all the flights

            return Ok(flights);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Flight>>> SearchFlights(
                string? from,
                string? to,
                DateTime? date)

        {
            var query = _context.Flights.AsQueryable(); //Start with all flights in the database and let me build a search on top of them.

            if (date.HasValue)
            {
                query = query.Where(f => f.DepartureTime.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(from)) //filter 
            {
                query = query.Where(f => f.From.ToLower() == from.ToLower());
            }

            if (!string.IsNullOrEmpty(to))
            {
                query = query.Where(f => f.To.ToLower() == to.ToLower());
            }

            var flights = await query.ToListAsync();

            return Ok(flights);
        }
    }
}