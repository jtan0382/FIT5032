using MHealth.Models.Domain;
using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MHealth.Repositories.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly RoleManager<IdentityRole> _roleManager;


        public AdminRepository(DatabaseContext _context, UserManager<UserModel> _userManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            //this._roleManager = _roleManager;
        }

        public async Task<Status> CreateStaff(SignupModel model)
        {
            var status = new Status();
            var userExists = await _userManager.FindByNameAsync(model.Username);
            var emailExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null || emailExists != null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User already exists";
            }
            else
            {
                UserModel user = new()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Username,
                    EmailConfirmed = true

                };
                await _userManager.CreateAsync(user, model.Password);
                status.StatusCode = 1;
            }
            

            return status;
        }

        public async Task<Status> DeleteUser(string id)
        {
            var status = new Status();

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                status.StatusCode = 1;
            }
            else
            {
                status.StatusCode = 0;
            }
            return status;
        }

        public async Task<IEnumerable<UserModel>> GetAllUser()
        {

            var nonAdminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            var allUsers = await _userManager.Users.ToListAsync();

            //var nonAdminUsersList = allUsers.Except(nonAdminUsers)
            //    .Where(user =>user.Name.ToLower().Contains(search) || // Search in the "Name" property
            //    user.UserName.ToLower().Contains(search) || // Search in the "UserName" property
            //    user.Email.ToLower().Contains(search)) // Search in the "email" property
            //    .OrderBy(user => user.UserName)
            //    .ToList();

            var nonAdminUsersList = allUsers.Except(nonAdminUsers)
                .OrderBy(user => user.UserName)
                .ToList();


            return nonAdminUsersList;

        }


        public async Task<UserModel> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);


        }

        //public async Task<IEnumerable<User>> SearchUser(string search, string user)
        //{
        //    if (user == "staff")
        //    {
        //        //staff
        //        var nonAdminUsers = await _userManager.GetUsersInRoleAsync("Admin");

        //        // Invert the result to get non-admin users
        //        var allUsers = await _userManager.Users.ToListAsync();
        //        var nonAdminUsersList = allUsers.Except(nonAdminUsers).ToList();

        //        var nonUserUsers = await _userManager.GetUsersInRoleAsync("User");
        //        var staffList = nonAdminUsersList.Except(nonUserUsers)
        //            .Where(user => user.Name.ToLower().Contains(search) || // Search in the "Name" property
        //            user.UserName.ToLower().Contains(search) || // Search in the "UserName" property
        //            user.Email.ToLower().Contains(search)) // Search in the "email" property
        //             .OrderBy(user => user.UserName)
        //            .ToList();

        //        return staffList;
        //    }
        //    else
        //    {
        //        //user
        //        var nonAdminUsers = await _userManager.GetUsersInRoleAsync("Admin");

        //        var allUsers = await _userManager.Users.ToListAsync();
        //        var nonAdminUsersList = allUsers.Except(nonAdminUsers)
        //            .Where(user => user.Name.ToLower().Contains(search) || // Search in the "Name" property
        //            user.UserName.ToLower().Contains(search) || // Search in the "UserName" property
        //            user.Email.ToLower().Contains(search)) // Search in the "email" property
        //            .OrderBy(user => user.UserName)
        //            .ToList();


        //        return nonAdminUsersList;
        //    }
            

            
        //}

        public async Task<Status> UpdateUser(UserModel model)
        {
            Status status = new Status();
            var users = await _context.Users.FindAsync(model.Id);
            Console.WriteLine(model.Id);
            
            if (users != null)
            {
                users.Name = model.Name;
                users.UserName = model.UserName;
                users.Email = model.Email;
                _context.Update(users);
                await _context.SaveChangesAsync();
                status.StatusCode = 1;
            }
            else
            {
                status.StatusCode = 0;
            }

            return status;
        }

    }
}
