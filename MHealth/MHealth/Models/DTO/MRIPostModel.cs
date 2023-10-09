using System.ComponentModel.DataAnnotations;

namespace MHealth.Models.DTO
{
    public class MRIPostModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        public string BookingId { get; set; }
        public DateTime PostDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PostPath { get; set; }
    }
}
