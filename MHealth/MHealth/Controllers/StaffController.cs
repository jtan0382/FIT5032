using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MHealth.Controllers
{
    [Authorize(Roles = "staff")]
    public class StaffController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IStaffRepository _staffRepository;
        private readonly UserManager<UserModel> _userManager;

        public StaffController(ILogger<BookingController> _logger, IStaffRepository _staffRepository, UserManager<UserModel> _userManager)
        {
            this._logger = _logger;
            this._staffRepository = _staffRepository;
            this._userManager = _userManager;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var bookings = await _staffRepository.GetAllBooking(user.Id);

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

    }
}
