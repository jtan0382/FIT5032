using System.ComponentModel.DataAnnotations;

namespace MHealth.Models.DTO
{
    public class BookingModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        [Required(ErrorMessage = "Booking Time is required.")]
        public DateTime BookingTime { get; set; }
        public int Status { get; set; }
    }
}
