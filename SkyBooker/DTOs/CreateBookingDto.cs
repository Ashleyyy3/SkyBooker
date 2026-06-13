using System.ComponentModel.DataAnnotations;

namespace SkyBooker.DTOs
{
    public class CreateBookingDto
    {
        //Users need to fill the needed infromation to create a booking. 
        [Required]
        public int FlightId { get; set; } //to change later ofc

        [Required]
        public string PassengerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string PassengerEmail { get; set; } = string.Empty;
    }
}