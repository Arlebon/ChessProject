using ChessProject.BLL.Services;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using ChessProject.Mappers;
using ChessProject.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChessProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddMember()
        {
            return View(new UserRegisterFormDTO());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddMember([FromForm] UserRegisterFormDTO form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            try
            {
                _userService.Register(form.ToUser());

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) 
            {
                return View(form);
            }
        }

        public IActionResult Login()
        {
            return View(new UserLoginFormDTO());
        }

        [HttpPost]
        public IActionResult Login([FromForm] UserLoginFormDTO form)
        {
            if (!ModelState.IsValid)
            {
                form.Password = "";
                return View(form);
            }
            try
            {
                User user = _userService.Login(form.Login, form.Password);

                ClaimsPrincipal claims = new ClaimsPrincipal(
                    new ClaimsIdentity([
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                        ], CookieAuthenticationDefaults.AuthenticationScheme)
                );

                HttpContext.SignInAsync(claims);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                form.Password = "";
                Console.WriteLine(ex.Message);
                return View(form);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Login", "User");
        }
    }
}
