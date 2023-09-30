using System.ComponentModel.DataAnnotations;

namespace MHealth.Models.Domain
{
    public class BookingModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        [Required]
        public DateTime BookingTime { get; set; }
        public int Status { get; set; }
    }
}
