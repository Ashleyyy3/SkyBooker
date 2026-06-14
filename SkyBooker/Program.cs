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

                if (!context.Flights.Any()) //does the flight table contain at least one row.IF FALSE flights in database, then add some flights
                {
                    context.Flights.AddRange(
                        new Flight
                        {
                            From = "Stockholm",
                            To = "London",
                            DepartureTime = new DateTime(2026, 7, 1, 8, 30, 0),
                            Price = 1299,
                            AvailableSeats = 50
                        },
                        new Flight
                        {
                            From = "Gothenburg",
                            To = "Paris",
                            DepartureTime = new DateTime(2026, 7, 2, 10, 0, 0),
                            Price = 1599,
                            AvailableSeats = 35
                        },
                        new Flight
                        {
                            From = "Malmö",
                            To = "Rome",
                            DepartureTime = new DateTime(2026, 7, 3, 14, 45, 0),
                            Price = 1899,
                            AvailableSeats = 20
                        }
                    );

                    context.SaveChanges();
                }
            }
            app.Run();
        }
    }
}
