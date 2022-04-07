using EduHomeFinal.DAL;
using EduHomeFinal.Extensions;
using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        #region UserManager
        private readonly UserManager<AppUser> _userManager;
        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName,
                    Email = user.Email,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);

            }
            return View(userVMs);
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
                return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest();
            if (user.IsDeactive)
            {
                user.IsDeactive = false;
            }
            else
            {
                user.IsDeactive = true;
            }


            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region ChangeRole
        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id==null)
            
                return BadRequest();
            
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user==null)
            
                return BadRequest();
            List<string> roles = new List<string>();
            roles.Add(Helper.Roles.Admin.ToString());
            roles.Add(Helper.Roles.Member.ToString());
            roles.Add(Helper.Roles.Moderator.ToString());
            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRole = new ChangeRoleVM
            {
                Username = user.UserName,
                Role = oldRole,
                Roles= roles

            };

            return View(changeRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id,string newRole)
        {
            if (id == null)

                return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)

                return BadRequest();
            List<string> roles = new List<string>();
            roles.Add(Helper.Roles.Admin.ToString());
            roles.Add(Helper.Roles.Member.ToString());
            roles.Add(Helper.Roles.Moderator.ToString());
            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRole = new ChangeRoleVM
            {
                Username = user.UserName,
                Role = oldRole,
                Roles = roles

            };
            IdentityResult addIdentityResult= await _userManager.AddToRoleAsync(user, newRole);
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("","Error");
                return View(changeRole);
            }
            IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
            if (!removeIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Error");
                return View(changeRole);
            }

            return RedirectToAction("Index");
        }
        #endregion


    }
}
