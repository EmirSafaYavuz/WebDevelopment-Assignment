using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;

public class AccountController : Controller
{
    private static List<User> Users = new List<User>();

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        if (Users.Any(u => u.Email == user.Email))
        {
            ModelState.AddModelError("Email", "Bu email adresi zaten kayıtlı.");
            return View(user);
        }

        Users.Add(user);
        TempData["Message"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = Users.FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            TempData["Message"] = $"Hoş geldiniz, {user.FullName}!";
            return RedirectToAction("Index", "Home");
        }

        TempData["Error"] = "Email veya şifre hatalı. Tekrar deneyin.";
        return View();
    }
}