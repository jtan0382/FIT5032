
using MHealth.Models.Domain;

namespace MHealth.Repositories.Abstract
{
    public interface IStaffRepository
    {
        Task<IEnumerable<BookingViewModel>> GetAllBooking(string staffId);
    }
}
