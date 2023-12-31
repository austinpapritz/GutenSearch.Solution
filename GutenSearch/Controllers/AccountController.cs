using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GutenSearch.Models;
using GutenSearch.ViewModels;
using System.Threading.Tasks;

namespace GutenSearch.Controllers
{
    public class AccountController : Controller
    {
        private readonly GutenSearchContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, GutenSearchContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patron");
                    bool loginSuccess = await AutoLoginUserAsync(user.UserName); // Automatically log in the user after successful registration
                    if (loginSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong when logging in. Please try again.");
                    return View(model);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        // Inside your AccountController:
        [HttpPost]
        public async Task<ActionResult> RegisterLibrarian(RegisterViewModel model)
        {
            // Registration code...
            var user = new ApplicationUser { UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Librarian");
                await AutoLoginUserAsync(user.UserName); // Automatically log in the user after successful registration
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        // Method to auto login after registering.
        private async Task<bool> AutoLoginUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return true;
            }
            return false;
        }
    }
}

