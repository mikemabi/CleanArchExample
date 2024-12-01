using CleanArchExample.Core.Entities;
using CleanArchExample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            //Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            //Find the user by the supplied username
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null)
            {
                //Attempt to signin with the provided username and password
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);
                if (result.Succeeded)
                {
                    //Add claims for the user that signed in
                    await AddClaimsForUser(user);


                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (roles.Contains("Visitor"))
                        {
                            return RedirectToAction("ProductsFromDb", "Products");
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password Incorrect");

                }
            }
            //Add an error 
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(loginModel);

        }

        private async Task AddClaimsForUser(ApplicationUser user)
        {
            var existingClaims = await _userManager.GetClaimsAsync(user);
            //Add Firstname claim if it doesn't exist
            var firstnameClaim = existingClaims.FirstOrDefault(c=>c.Type == "firstname");
            if (firstnameClaim == null) 
            {
                firstnameClaim = new Claim("firstname", user.FirstName);
                await _userManager.AddClaimAsync(user, firstnameClaim);
            }
            //Add Lastname claim if it doesn't exist
            var lastnameClaim = existingClaims.FirstOrDefault(c => c.Type == "lastname");
            if (lastnameClaim == null)
            {
                lastnameClaim = new Claim("lastname", user.LastName);
                await _userManager.AddClaimAsync(user, lastnameClaim);
            }
            //Refresh the signin to apply new claims
            await _signInManager.RefreshSignInAsync(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //create a new user with the provided details
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            //Attempt to create athe user with the provided password
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                //Add any errors from the result to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            //Ensure the visitor role exists and add this user to that role
            if (!await _roleManager.RoleExistsAsync("Visitor"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Visitor" });
            }
            await _userManager.AddToRoleAsync(user, "Visitor");
            //Redirect to the login page after succesful registeration
            return RedirectToAction("Login");
        }
    }
}
