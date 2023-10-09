using MHealth.Models.Domain;
using MHealth.Models.Domain.View;
using MHealth.Models.DTO;

namespace MHealth.Repositories.Abstract
{
    public interface IBookingRepository
    {
        Task Booking(string userId, string staffId, DateTime bookingTime);
        Task<IEnumerable<BookingViewModel>> GetAllBooking(string userId);
        Task<MRIViewModel> GetBookingInformation(string bookingId, string userId, string staffId);
        //Task<MRIViewModel> GetBookingInformation(string mriId);
    }
}
