using MHealth.Models.Domain;
using MHealth.Models.DTO;

namespace MHealth.Repositories.Abstract
{
    public interface IAdminRepository
    {
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<UserModel> GetUserById(string id);
        Task<Status> UpdateUser(UserModel model);
        Task<Status> DeleteUser(string id);
        Task<Status> CreateStaff(SignupModel model);
    
    }
}
