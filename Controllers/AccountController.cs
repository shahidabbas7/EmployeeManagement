using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                 UserName=model.Email,
                 Email=model.Email,
                 City=model.City
                };
                var result =await userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    if (await userManager.IsInRoleAsync(user,"Admin"))
                    {
                        return RedirectToAction("index", "home");
                    }
                    else
                    {
                       await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("index", "home");
                    }
                   
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> Isemailvalid(string email)
        {
          var result=  await userManager.FindByEmailAsync(email);
            if (result == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use ");
            }
        }
        [AllowAnonymous]
        public IActionResult login( string Returnurl)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> login(LoginVM model,  string Returnurl)
        {

            if (ModelState.IsValid)
            {
               
                var result = await signInManager.PasswordSignInAsync(model.email,model.password,model.remeberme,false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Returnurl)&& Url.IsLocalUrl(Returnurl))
                    {
                        return Redirect(Returnurl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                }
            }
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
       
    }
}
