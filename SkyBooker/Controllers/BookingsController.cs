using Microsoft.AspNetCore.Mvc;
using SkyBooker.Models;

namespace SkyBooker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        [HttpPost]// Creates data
        public ActionResult<Booking> CreateBooking(Booking booking)
        {
            booking.Id = 1;

            return Ok(booking);
        }
    }
}