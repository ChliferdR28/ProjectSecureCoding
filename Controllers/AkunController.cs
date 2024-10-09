using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjectSecureCoding.Data;
using ProjectSecureCoding.Models;
using ProjectSecureCoding.ViewModels;

namespace ProjectSecureCoding.Controllers
{
    public class AkunController : Controller
    {
        
         private readonly IUser _userData;

        public AkunController(IUser user)
        {
            _userData = user;
        }

        private User GetCurrentUser()
        {
            var username = User.Identity.Name; // Use Name to get the logged-in user's username
            if (string.IsNullOrEmpty(username))
            {
                return null; // Return null if the user is not authenticated
            }

            // Retrieve the user from the database
            return _userData.GetUserByUsername(username);
        }

        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = loginViewModel.Username,
                        Password = loginViewModel.Password
                    };

                    var loginUser = _userData.Login(user);
                    if (loginUser == null)
                    {
                        ViewBag.Error = "Invalid login Attempted"; // Pesan kesalahan jika login gagal
                        return View(loginViewModel);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginUser.Username), // Pastikan menggunakan loginUser
                        new Claim(ClaimTypes.Role, loginUser.Role) // Pastikan role ditambahkan
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = loginViewModel.RememberLogin
                        });

                    // Login berhasil, redirect ke dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "ModelState not valid"; // Jika model tidak valid
                }
            }
            catch (Exception ex)
            {
                // Login gagal, tampilkan error
                ViewBag.Error = ex.Message;
            }
            return View(loginViewModel);
        }

        [HttpGet("register")]
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("register")]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = registrationViewModel.Username,
                        Password = registrationViewModel.Password, // Pastikan ini password yang sudah di-hash
                        Role = "contributor"
                    };
                    _userData.Registration(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Registration failed, show error
                ViewBag.Error = ex.Message;
            }
            return View(registrationViewModel);
        }
    }
}