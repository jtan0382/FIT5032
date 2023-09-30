namespace MHealth.Repositories.Abstract
{
    public interface IBookingRepository
    {
        Task Booking(string userId, string staffId, DateTime bookingTime);
    }
}
