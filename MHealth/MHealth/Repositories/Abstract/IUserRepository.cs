using MHealth.Models.Domain;

namespace MHealth.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllStaff();
    }
}
