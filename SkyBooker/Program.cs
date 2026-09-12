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

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Flights.Any(f => f.Airline != ""))
                {
                    context.Flights.AddRange(
                        new Flight
                        {
                            From = "Gothenburg",
                            To = "Paris",
                            Airline = "SAS",
                            AirlineCode = "SK",
                            DepartureAirportCode = "GOT",
                            ArrivalAirportCode = "CDG",
                            DepartureTime =
                                new DateTime(2026, 11, 15, 7, 45, 0),
                            ArrivalTime =
                                new DateTime(2026, 11, 15, 10, 5, 0),
                            Price = 1599,
                            Stops = 0,
                            Provider = "SAS",
                            BookingUrl = "https://www.flysas.com/",
                            AvailableSeats = 50
                        },

                        new Flight
                        {
                            From = "Gothenburg",
                            To = "Paris",
                            Airline = "KLM",
                            AirlineCode = "KL",
                            DepartureAirportCode = "GOT",
                            ArrivalAirportCode = "CDG",
                            DepartureTime =
                                new DateTime(2026, 11, 15, 9, 10, 0),
                            ArrivalTime =
                                new DateTime(2026, 11, 15, 13, 20, 0),
                            Price = 1349,
                            Stops = 1,
                            Provider = "KLM",
                            BookingUrl = "https://www.klm.se/",
                            AvailableSeats = 28
                        },

                        new Flight
                        {
                            From = "Gothenburg",
                            To = "Paris",
                            Airline = "Lufthansa",
                            AirlineCode = "LH",
                            DepartureAirportCode = "GOT",
                            ArrivalAirportCode = "CDG",
                            DepartureTime =
                                new DateTime(2026, 11, 15, 14, 30, 0),
                            ArrivalTime =
                                new DateTime(2026, 11, 15, 18, 40, 0),
                            Price = 1199,
                            Stops = 1,
                            Provider = "Lufthansa",
                            BookingUrl = "https://www.lufthansa.com/",
                            AvailableSeats = 35
                        }
                    );

                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}