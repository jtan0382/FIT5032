﻿using MHealth.Models.Domain;
using MHealth.Models.DTO;
using MHealth.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MHealth.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;

        public AdminController(ILogger<AdminController> _logger, IAdminRepository _adminRepository)
        {
            this._logger = _logger;
            this._adminRepository = _adminRepository;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            //search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            var users = await _adminRepository.GetAllUser();
            var totalCount = users.Count();
            int pageSize = 5;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            List<UserModel> userList = users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            UserPaginationViewModel userData = new UserPaginationViewModel()
            {
                Users = userList,
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize
            };
            return View(userData);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {

            UserModel user = await _adminRepository.GetUserById(id);
            if (user != null)
            {
                return View(user); // Pass the user data to the view
            }
            //TempData["msg"] = $"User is not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _adminRepository.UpdateUser(model);
                    //TempData["code"] = "1";
                    //TempData["msg"] = "The user updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //TempData["code"] = "0";
                    //TempData["msg"] = "The user is failed to be updated";
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                UserModel user = await _adminRepository.GetUserById(id);

                if (user != null)
                {
                    try
                    {
                        await _adminRepository.DeleteUser(id);
                        //return RedirectToAction(nameof(Index));

                        //TempData["msg"] = "The user deleted successfully";
                    }
                    catch (Exception ex)
                    {
                        //TempData["msg"] = ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                //TempData["msg"] = ex.Message;

            }
            TempData["msg"] = $"User is not found with Id : {id}";
            return RedirectToAction(nameof(Index));

        }

        public IActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(SignupModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "staff";
            try
            {
                await _adminRepository.CreateStaff(model);
                //TempData["msg"] = "The user created successfully";
                //TempData["name"] = User.Identity.Name;
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                //TempData["msg"] = "The user is failed to be updated";

                return RedirectToAction(nameof(CreateStaff));
            }

        }

    }
}

        //public async Task<IActionResult> SearchUser(string search)
        //{
        //    search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
        //    var userList = await _adminRepository.SearchUser(search, "user");

        //    return View(userData);
        //}


//    }
//}
