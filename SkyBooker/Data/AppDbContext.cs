using Microsoft.EntityFrameworkCore;
using SkyBooker.Models;

//The bridge between the models and the database so C# --> SQL sever tabeles

namespace SkyBooker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Booking> Bookings { get; set; }
    }
}