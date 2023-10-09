using MHealth.Migrations;
using MHealth.Models.Domain;
using MHealth.Models.Domain.View;
using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MHealth.Repositories.Implementation
{
    [Authorize(Roles = "user")]
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
            bool isBookingConflictByStaff = await _context.Bookings.AnyAsync(b =>
                b.StaffId == staffId &&
                b.BookingTime == bookingTime);

            bool isBookingConflictByAppointment = await _context.Bookings.AnyAsync(b =>
                    b.UserId == userId &&
                    b.BookingTime == bookingTime);

            if (!isBookingConflictByStaff && !isBookingConflictByAppointment) {
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

        public async Task<IEnumerable<BookingViewModel>> GetAllBooking(string userId)
        {
            var bookings = await _context.Bookings.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var mriPosts = await _context.MRIPosts.ToListAsync();
            var bookingList = new List<BookingViewModel>();

            try
            {
                var userBooking =
                                from booking in bookings
                                join user in users on booking.UserId equals user.Id
                                join staff in users on booking.StaffId equals staff.Id
                                where user.Id == userId
                                select new BookingViewModel
                                {
                                    Id = booking.Id,
                                    UserId = user.Id,
                                    StaffId = booking.StaffId,
                                    UserName = user.UserName,
                                    StaffName = staff.UserName,
                                    BookingTime = booking.BookingTime,
                                    Status = booking.Status
                                };

                bookingList = userBooking
                    .Where(b => b.Status != 2)
                    .OrderBy(b => b.BookingTime)
                    .Cast<BookingViewModel>()
                    .ToList();
            }
            catch (Exception ex)
            {

            }

            return bookingList;
        }

        public async Task<MRIViewModel> GetBookingInformation(string bookingId, string userId, string staffId)
        {
            try
            {
                // Find the booking entity by its composite key
                var booking = await _context.Bookings.FindAsync(bookingId, userId, staffId);

                if (booking != null)
                {
                    // Use LINQ to join the booking entity with other tables
                    var mriViewModel = from b in new List<BookingModel> { booking } // Create a list with the found booking
                                       join mripost in _context.MRIPosts on b.Id equals mripost.BookingId
                                       join user in _context.Users on b.UserId equals user.Id
                                       join staff in _context.Users on b.StaffId equals staff.Id
                                       select new MRIViewModel
                                       {
                                           BookingId = booking.Id,
                                           UserId = user.Id,
                                           StaffId = staff.Id,
                                           UserName = user.UserName,
                                           StaffName = staff.UserName,
                                           Description = mripost.Description,  // Include user email if needed
                                           PostPath = mripost.PostPath,  // Include staff email if needed
                                           Status = booking.Status,
                                           BookingTime = booking.BookingTime,
                                           //MRITime = mripost.PostDate
                                       };

                    // Retrieve the first matching result or null
                    var mri = mriViewModel.FirstOrDefault();

                    return mri;
                }
                else
                {
                    // Handle the case where the booking with the specified composite key does not exist
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
                Console.WriteLine(ex.Message);
                return null; // Return null in case of an exception
            }
        }

            //public async Task<MRIViewModel> GetBookingInformation(string bookingId, string userId, string staffId)
            //{
            //    var bookings = await _context.Bookings.ToListAsync();
            //    var users = await _context.Users.ToListAsync();
            //    var mriPosts = await _context.MRIPosts.ToListAsync();
            //    MRIViewModel mri = new MRIViewModel();

            //    try
            //    {

            //        var mriViewModel = from booking in bookings
            //                           join mripost in mriPosts on booking.Id equals mripost.BookingId
            //                           join user in users on booking.UserId equals user.Id
            //                           join staff in users on booking.StaffId equals staff.Id
            //                           where booking.Id == bookingId && booking.UserId == userId && booking.StaffId == staffId
            //                           select new MRIViewModel
            //                           {
            //                               BookingId = booking.Id,
            //                               UserId = user.Id,
            //                               StaffId = staff.Id,
            //                               UserName = user.UserName,
            //                               StaffName = staff.UserName,
            //                               //Description = mripost.Description,  // Include user email if needed
            //                               //PostPath = mripost.PostPath,  // Include staff email if needed
            //                               Status = booking.Status,
            //                               BookingTime = booking.BookingTime,
            //                               //MRITime = mripost.PostDate
            //                           };
            //        Console.WriteLine(mriViewModel.ToString());
            //        mri = mriViewModel.FirstOrDefault();
            //        //mri = mriViewModel.FirstOrDefault(book => book.Id == bookingId && book.UserId == userId && book.StaffId == staffId);
            //    }
            //    catch (Exception ex) { }
            //    return mri;
            //}
        }
}
