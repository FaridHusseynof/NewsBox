using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsBox.Models;
using NewsBox.ViewModels;

namespace NewsBox.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM user)
        {
            if (!ModelState.IsValid) return View(user);
            AppUser newUser = new AppUser 
            {
            Email=user.Email,
            FullName=user.FullName,
            UserName=user.UserName
            };
            var identityResult = await _userManager.CreateAsync(newUser, user.Password);
            if (!identityResult.Succeeded) 
            {
                foreach (var err in identityResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(user);
            }
            await _userManager.AddToRoleAsync(newUser, "Member");
            await _signInManager.SignInAsync(newUser, true);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid) return View(user);
            AppUser? newUser = await _userManager.FindByEmailAsync(user.Email);
            if (newUser==null)
            {
                ModelState.AddModelError("", "Email or password is incorrect.");
                return View(user);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(newUser, user.Password, true, true);
            if (signInResult.IsLockedOut) 
            {
                ModelState.AddModelError("", "Try again later.");
                return View(user);
            }
            
            if (!signInResult.Succeeded) 
            {
                ModelState.AddModelError("", "Email or password is incorrect.");
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles() 
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin")) 
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Member"));
            }
            return Content("Roles Created");

        }
    }
}
