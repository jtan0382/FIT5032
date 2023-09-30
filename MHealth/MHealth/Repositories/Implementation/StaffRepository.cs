using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MHealth.Repositories.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;

        public StaffRepository(DatabaseContext _context, UserManager<UserModel> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
        }

        public async Task<IEnumerable<BookingViewModel>> GetAllBooking(string staffId)
        {
            var bookings = await _context.Bookings.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var bookingList = new List<BookingViewModel>();

            try
            {
                var userBooking =
                                from booking in bookings
                                join user in users on booking.UserId equals user.Id
                                select new BookingViewModel
                                {
                                    Id = booking.Id,
                                    StaffId = booking.StaffId,
                                    UserName = user.UserName,
                                    BookingTime = booking.BookingTime
                                };

                bookingList = userBooking
                    .OrderBy(b => b.BookingTime)
                    .Cast<BookingViewModel>()
                    .ToList();
            }
            catch (Exception ex){

            }

            return bookingList;

        }

    }
}
