using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using GitServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GitServer.Controllers;

public class UserController : Controller
{
    private IRepository<User> _user;
    public UserController(IRepository<User> user)
    {
        _user = user;
    }
    public IActionResult Index()
    {
        return Content("ok");
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _user.List(r => r.Name == model.Username && r.Password == model.Password).FirstOrDefault();
            if (user != null)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Redirect("/");
            }
        }
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            _user.Add(new User()
            {
                Name = model.Username,
                Email = model.Email,
                Password = model.ConfirmPassword,
                CreationDate = DateTime.Now
            });
            return Redirect("/");
        }
        return View();
    }
    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }

}