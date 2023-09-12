using MHealth.Models.Domain;
using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MHealth.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Username or password is wrong";
                return status;
            }
            else
            {
                if (!await userManager.CheckPasswordAsync(user, model.Password))
                {
                    status.StatusCode = 0;
                    status.StatusMessage = "Username or password is wrong";
                    return status;
                }
                else
                {
                    var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);

                    if (signInResult.Succeeded)
                    {
                        var userRoles = await userManager.GetRolesAsync(user);
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName)
                        };
                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        status.StatusCode = 1;
                        status.StatusMessage = "Logged in succcessfully";
                        return status;

                    }

                    else if (signInResult.IsLockedOut)
                    {
                        status.StatusCode = 0;
                        status.StatusMessage = "User locked out";
                        return status;
                    }
                    else
                    {
                        status.StatusCode = 0;
                        status.StatusMessage = "Error on logging in";
                        return status;
                    }
                }
            }

            //Match the password
            
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegistrationAsync(SignupModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User already exists";
                return status;
            }

            User user = new User
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.Username,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User creation failed";
                return status;
            }

            // role management
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.StatusMessage = "User has registered successfully";
            return status;
        }
    }
}
