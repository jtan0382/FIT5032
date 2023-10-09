using MHealth.Models.Domain;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MHealth.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserRepository(DatabaseContext _context, UserManager<UserModel> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }
        public async Task<IEnumerable<UserModel>> GetAllStaff()
        {
            var nonAdminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            // Invert the result to get non-admin users
            var allUsers = await _userManager.Users.ToListAsync();
            var nonAdminUsersList = allUsers.Except(nonAdminUsers).OrderBy(user => user.UserName).ToList();

            var nonUserUsers = await _userManager.GetUsersInRoleAsync("User");
            var staffList = nonAdminUsersList.Except(nonUserUsers)
                .OrderBy(user => user.UserName)
                .ToList();

            return staffList;

        }
    }
}
