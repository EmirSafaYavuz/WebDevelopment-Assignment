using Assignment.Models;
using Assignment.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers;

public class AccountController : Controller
{
    private readonly ICustomerService _customerService;

    public AccountController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Register(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        try
        {
            // Check if the email is already in use
            var existingCustomer = (await _customerService.GetAllCustomersAsync())
                .FirstOrDefault(c => c.Email == customer.Email);

            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "Bu email zaten kayıtlı");
                return View(customer);
            }

            // Hash the password before saving
            customer.PasswordHash = HashPassword(customer.PasswordHash);

            await _customerService.AddCustomerAsync(customer);

            // Set success message
            TempData["StatusMessage"] = "Kayıt başarılı, giriş yapabilirsiniz.";
            TempData["StatusType"] = "success"; 
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            TempData["StatusMessage"] = "Bir hata meydana geldi. Lütfen daha sonra tekrar deneyin.";
            TempData["StatusType"] = "error"; 
            return View(customer);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        if (Request.Cookies.TryGetValue("UserCredentials", out var userCredentials))
        {
            var credentials = userCredentials.Split('|');
            var email = credentials[0];
            var password = credentials[1];

            var users = await _customerService.GetAllCustomersAsync();
            var user = users.FirstOrDefault(u => u.Email == email && u.PasswordHash == HashPassword(password));

            if (user != null)
            {
                TempData["Message"] = $"Hoşgeldin, {user.FirstName}!";
                return RedirectToAction("Index", "Home");
            }
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _customerService.GetAllCustomersAsync();
        var validUser = user.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == HashPassword(model.Password));

        if (validUser != null)
        {
            if (model.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Prevent client-side access to cookies
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7) // Cookie expiration
                };

                // Store email and password in a single cookie
                Response.Cookies.Append("UserCredentials", $"{model.Email}|{model.Password}", cookieOptions);
            }

            TempData["Message"] = $"Hoş Geldin, {validUser.FirstName}!";
            return RedirectToAction("Index", "Home");
        }

        TempData["Error"] = "Invalid email or password.";
        return View(model);
    }
    
    [HttpPost]
    public IActionResult Logout()
    {
        // Remove the "UserCredentials" cookie
        Response.Cookies.Delete("UserCredentials");

        TempData["Message"] = "You have been logged out.";
        return RedirectToAction("Login");
    }

    private string HashPassword(string password)
    {
        return password; // Şimdilik hashlemeden döndürüyor
    }
}