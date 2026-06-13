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
    }
}