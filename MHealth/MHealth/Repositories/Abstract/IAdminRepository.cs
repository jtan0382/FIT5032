using MHealth.Models.Domain;
using MHealth.Models.Domain.View;
using MHealth.Models.DTO;

namespace MHealth.Repositories.Abstract
{
    public interface IAdminRepository
    {
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<IEnumerable<BookingViewModel>> GetAllBooking();
        Task<UserModel> GetUserById(string id);
        Task<BookingModel> GetBookingById(string id);
        Task<Status> UpdateUser(UserViewModel model);
        Task<Status> DeleteUser(string id);
        Task<Status> DeleteBooking(string id);
        Task<Status> CreateStaff(SignupModel model);
        //Task<UserModel> ConvertFromUserViewModel(UserViewModel model);
        //Task<UserViewModel> UserModelToUserViewModel(UserModel model);
        public Task<UserViewModel> UserModelToUserViewModel(string id);
    }
}
