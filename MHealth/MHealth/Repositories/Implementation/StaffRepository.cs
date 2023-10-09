using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MHealth.Models.Domain.View;
using MHealth.Models.DTO;
using System;
using MHealth.Migrations;

namespace MHealth.Repositories.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _environment;

        public StaffRepository(DatabaseContext _context, UserManager<UserModel> _userManager, IWebHostEnvironment _environment)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._environment = _environment;
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
                                join staff in users on booking.StaffId equals staff.Id
                                where staff.Id == staffId

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
                    .Where(b => b.Status == 0)
                    .OrderBy(b => b.BookingTime)
                    .Cast<BookingViewModel>()
                    .ToList();
            }
            catch (Exception ex){

            }

            return bookingList;

        }

        public async Task<BookingViewModel> GetBookingDetail(string bookingId)
        {
            var bookings = _context.Bookings.ToList();
            var users = _context.Users.ToList();
            BookingViewModel book = new BookingViewModel(); // Initialize to null

            try
            {
                var userBooking =
                    from booking in bookings
                    join user in users on booking.UserId equals user.Id
                    join staff in users on booking.StaffId equals staff.Id

                    select new BookingViewModel
                    {
                        Id = booking.Id,
                        UserId = user.Id,
                        StaffId = booking.StaffId,
                        UserName = user.UserName,
                        StaffName = staff.UserName,
                        UserEmail = user.Email,
                        StaffEmail = staff.Email,
                        BookingTime = booking.BookingTime,
                        Status = booking.Status
                    };
                book = userBooking.FirstOrDefault(book => book.Id == bookingId);
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }

            return book;
        }

        public async Task<UserModel> GetUserInformation(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task Post(MRIPostModel model)
        {
            try
            {
                MRIPostModel post = new MRIPostModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    BookingId = model.BookingId,
                    UserId = model.UserId,
                    StaffId = model.StaffId,
                    Description = model.Description,
                    PostDate = DateTime.Today,
                    PostPath = model.PostPath
                };
                _context.MRIPosts.Add(post);
                //await _context.SaveChangesAsync();

                var booking = await _context.Bookings.FindAsync(model.BookingId, model.UserId, model.StaffId);
                if (booking != null)
                {
                    booking.Status = 1;
                    _context.Update(booking);
                    await _context.SaveChangesAsync();


                }
            }
            catch (Exception ex)
            {

            }
        }


        //public async Task UploadFile(MRIPostModel model, IFormFile postImage)
        //{
        //    //Status status = new Status();
        //    string path = "";
        //    try
        //    {
        //        if (postImage.Length > 0)
        //        {
        //            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            using (var fileStream = new FileStream(Path.Combine(path, postImage.FileName), FileMode.Create))
        //            {
        //                await postImage.CopyToAsync(fileStream);
        //            }
        //            //status.StatusCode = 1;
        //        }
        //        else
        //        {
        //            //status.StatusCode = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("File Copy Failed", ex);
        //    }
        //}

        public async Task<MRIPostModel> WriteFile(IFormFile postImage)
        {
            MRIPostModel postModel = new MRIPostModel();
            string filename = "";

            var extension = "." + postImage.FileName.Split('.')[postImage.FileName.Split('.').Length - 1];
            filename = DateTime.Now.Ticks.ToString() + extension;

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
            postModel.PostPath = exactpath;
            using (var stream = new FileStream(exactpath, FileMode.Create))
            {
                await postImage.CopyToAsync(stream);
            }
            return postModel;
        }
    }
}
