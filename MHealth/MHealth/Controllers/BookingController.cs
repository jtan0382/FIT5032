using MHealth.Models.Domain;
using MHealth.Models.Domain.View;
using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using MHealth.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Globalization;

namespace MHealth.Controllers
{
    [Authorize]
    [Authorize(Roles = "user")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<UserModel> _userManager;

        public BookingController(IBookingRepository _bookingRepository, UserManager<UserModel> _userManager)
        {
            this._bookingRepository = _bookingRepository;
            this._userManager = _userManager;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var bookings = await _bookingRepository.GetAllBooking(user.Id);

                var totalCount = bookings.Count();
                int pageSize = 5;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                List<BookingViewModel> bookingList = bookings.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                BookingPaginationViewModel bookingData = new BookingPaginationViewModel()
                {
                    Bookings = bookingList,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    PageSize = pageSize
                };
                return View(bookingData);
            }
            catch (Exception ex)
            {
                TempData["error"] = "1";
            }
            return View();
        }




        public async Task<IActionResult> Booking(string id)
        {

            BookingModel bookingModel = new BookingModel
            {
                StaffId = id // Set the id in the BookingModel
            };

            return View(bookingModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(string id, string selectedDate)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                DateTime bookingTime = DateTime.Parse(selectedDate);
                //DateTime dateOnly = new DateTime(bookingTime.Year, bookingTime.Month, bookingTime.Day, 0, 0, 0);
                //TempData["selectedDate"] = bookingTime;
                try
                {

                    await _bookingRepository.Booking(user.Id, id, bookingTime);
                    return RedirectToAction("Staff", "Home");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Booking", "Booking");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Booking", "Booking");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Detail(string bookingId, string userId, string staffId)
        {
            MRIViewModel mri = await _bookingRepository.GetBookingInformation(bookingId, userId, staffId);
            if(mri == null)
            {
                TempData["error"] = "1";
            }
            return View(mri);
        }

    }
}
