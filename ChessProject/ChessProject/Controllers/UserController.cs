using ChessProject.BLL.Services;
using ChessProject.Mappers;
using ChessProject.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ChessProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult AddMember()
        {
            return View(new UserRegisterFormDTO());
        }

        [HttpPost]
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
            catch
            {
                return View(form);
            }
        }
    }
}
