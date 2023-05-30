using Bilet5.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bilet5.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }




        public IActionResult Register()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if(!ModelState.IsValid) { return View(register); }
            IdentityUser identityUser = new IdentityUser
            {
                UserName = register.UserName,
                Email = register.Email,
            };
           IdentityResult identityResult =await _userManager.CreateAsync(identityUser,register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(register);
            }
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login,string? ReturnUrl)
        {
            if (!ModelState.IsValid) { return View(login); }
            IdentityUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(login);

            }
            var signinResult =await _signInManager.CheckPasswordSignInAsync(user,login.Password,true);
            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(login);
            }
            await _signInManager.SignInAsync(user, login.RememberMe);
            if(Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

           
            return RedirectToAction("Index","Home");
        }



    }
}
