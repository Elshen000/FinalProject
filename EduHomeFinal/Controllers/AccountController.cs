using EduHomeFinal.Models;
using EduHomeFinal.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EduHomeFinal.Extensions.Helper;

namespace EduHomeFinal.Controllers
{
    public class AccountController : Controller
    {
        #region DbContext
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
       
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager
                                 )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
          
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View();
            AppUser appUser;
            appUser = await _userManager.FindByNameAsync(loginVM.Username);
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.Username);
                if (appUser == null)
                {
                    ModelState.AddModelError("", "İstifadəçi adı,Email və ya Parol yanlışdır");
                    return View();
                }
               

            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("", "Hesabınız silinmişdir ");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabınız bloklanmışdır. Zəhmət olmasa 10 dəqiqə gözləyin ");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "İstifadəçi adı,Email və ya Parol yanlışdır ");
                return View();
            }
           
            if (User.IsInRole("Admin"))
            {

                return RedirectToAction("Index", "Dashboard", new { area = "" });
            }
            if (User.IsInRole("Moderator"))
            {

                return RedirectToAction("Index", "ModeratorCourse", new { area = "" });
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email


            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View();
            }
            //_userManager.GenerateEmailConfirmationTokenAsync()
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser, true);
            return RedirectToAction("Index", "Home");

        }
        public async Task CreateRole()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Roles.Member.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Roles.Moderator.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Moderator.ToString() });
            }


        }
        #endregion

        #region ForgotPassword
        public IActionResult ForgotPassword()
        {
           
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);
                    //await _userManager.SendEmailAsync(token, "Reset Password", "Please reset your password by click <a href=\"" + passwordResetlink+"\">here</a>");
                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Guliyev", "elwenquliyev513@gmail.com"));

                    message.To.Add(MailboxAddress.Parse(user.Email));
                    message.Body = new TextPart("plain")
                    {
                        Text = passwordResetLink
                    };
                    message.Subject = "Reset your password please";
                    string email = "elwenquliyev513@gmail.com";
                    string password = "guliyev513";
                    SmtpClient client = new SmtpClient();
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(email, password);
                    client.Send(message);
                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(forgotPasswordVM);
        }
        #endregion

        #region ResetPassword
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null && email == null)
            {
                ModelState.AddModelError("", "Password cannot be empty!");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(resetPasswordVM);
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(resetPasswordVM);
        }
        #endregion





    }
}
