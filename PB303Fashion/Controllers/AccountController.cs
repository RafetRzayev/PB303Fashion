using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PB303Fashion.DataAccessLayer;
using PB303Fashion.DataAccessLayer.Entities;
using PB303Fashion.Models;

namespace PB303Fashion.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                ModelState.AddModelError("", "Bu adda istifadeci movcuddur!");

                return View();
            }

            var createdUser = new AppUser
            {
                Fullname = model.Fullname,
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(createdUser, model.Password);

            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(model.Username);

            if (existUser == null)
            {
                ModelState.AddModelError("", "Username or password incorrert");

                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, true, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "You are blocked");

                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrert");

                return View();
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}
