using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MHealth.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _context;

        public BookingRepository(DatabaseContext _context)
        {
            this._context = _context;
        }

        public async Task Booking(string userId, string staffId, DateTime bookingTime)
        {
            // Check if there are any existing bookings for the selected staff member at the same time.
            bool isBookingConflict = await _context.Bookings.AnyAsync(b =>
                b.StaffId == staffId &&
                b.BookingTime == bookingTime);

            if (!isBookingConflict) {
                BookingModel booking = new BookingModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    StaffId = staffId,
                    BookingTime = bookingTime,
                    Status = 0
                };
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
            }

        }
    }
}
