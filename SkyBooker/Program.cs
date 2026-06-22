using Microsoft.EntityFrameworkCore;
using SkyBooker.Data;
using SkyBooker.Models;

namespace SkyBooker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Let's localhost load html
            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Flights.Any())
                {
                    context.Flights.AddRange(
                        // Stockholm ? London
                        new Flight { From = "Stockholm", To = "London", DepartureTime = new DateTime(2026, 7, 1, 8, 30, 0), Price = 1299, AvailableSeats = 48 },
                        new Flight { From = "Stockholm", To = "London", DepartureTime = new DateTime(2026, 7, 1, 12, 15, 0), Price = 1499, AvailableSeats = 35 },
                        new Flight { From = "Stockholm", To = "London", DepartureTime = new DateTime(2026, 7, 1, 18, 45, 0), Price = 999, AvailableSeats = 22 },
                        new Flight { From = "Stockholm", To = "London", DepartureTime = new DateTime(2026, 7, 2, 9, 0, 0), Price = 1399, AvailableSeats = 40 },
                        new Flight { From = "Stockholm", To = "London", DepartureTime = new DateTime(2026, 7, 3, 16, 20, 0), Price = 1199, AvailableSeats = 30 },

                        // Gothenburg ? Paris
                        new Flight { From = "Gothenburg", To = "Paris", DepartureTime = new DateTime(2026, 7, 1, 7, 45, 0), Price = 1599, AvailableSeats = 50 },
                        new Flight { From = "Gothenburg", To = "Paris", DepartureTime = new DateTime(2026, 7, 1, 14, 30, 0), Price = 1349, AvailableSeats = 28 },
                        new Flight { From = "Gothenburg", To = "Paris", DepartureTime = new DateTime(2026, 7, 2, 11, 0, 0), Price = 1449, AvailableSeats = 42 },

                        // Malmö ? Rome
                        new Flight { From = "Malmö", To = "Rome", DepartureTime = new DateTime(2026, 7, 1, 6, 15, 0), Price = 1799, AvailableSeats = 33 },
                        new Flight { From = "Malmö", To = "Rome", DepartureTime = new DateTime(2026, 7, 1, 15, 0, 0), Price = 1649, AvailableSeats = 45 },
                        new Flight { From = "Malmö", To = "Rome", DepartureTime = new DateTime(2026, 7, 3, 9, 30, 0), Price = 1899, AvailableSeats = 20 },

                        // Stockholm ? Barcelona
                        new Flight { From = "Stockholm", To = "Barcelona", DepartureTime = new DateTime(2026, 7, 1, 10, 0, 0), Price = 1699, AvailableSeats = 38 },
                        new Flight { From = "Stockholm", To = "Barcelona", DepartureTime = new DateTime(2026, 7, 1, 17, 30, 0), Price = 1449, AvailableSeats = 52 },
                        new Flight { From = "Stockholm", To = "Barcelona", DepartureTime = new DateTime(2026, 7, 2, 8, 0, 0), Price = 1549, AvailableSeats = 29 },

                        // Copenhagen ? Amsterdam
                        new Flight { From = "Copenhagen", To = "Amsterdam", DepartureTime = new DateTime(2026, 7, 1, 9, 15, 0), Price = 899, AvailableSeats = 60 },
                        new Flight { From = "Copenhagen", To = "Amsterdam", DepartureTime = new DateTime(2026, 7, 1, 13, 45, 0), Price = 749, AvailableSeats = 44 },
                        new Flight { From = "Copenhagen", To = "Amsterdam", DepartureTime = new DateTime(2026, 7, 2, 16, 0, 0), Price = 849, AvailableSeats = 37 }
                    );
                    context.SaveChanges();
                }
            }
            app.Run();
        }
    }
}
