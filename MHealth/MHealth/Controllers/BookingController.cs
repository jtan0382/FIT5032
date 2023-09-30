using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Globalization;

namespace MHealth.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _service;
        private readonly UserManager<UserModel> _userManager;

        public BookingController(IBookingRepository service, UserManager<UserModel> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
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

                    await _service.Booking(user.Id, id, bookingTime);
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
            //var result = await _service.Booking(user.Id, id, selectedDate.);

            return RedirectToAction("Staff", "Home");

        }

    }
}
