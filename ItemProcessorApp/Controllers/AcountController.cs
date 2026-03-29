using Microsoft.AspNetCore.Mvc;
using ItemProcessorApp.Data;
using ItemProcessorApp.Models;
using System.Linq;

namespace ItemProcessorApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Enter username and password";
                return View();
            }

            username = username.Trim().ToLower();
            password = password.Trim();

            var user = _context.Users
                .FirstOrDefault(u => u.Username.ToLower() == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("User", user.Username);
                return RedirectToAction("Index", "Item");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            System.Console.WriteLine("REGISTER HIT");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and Password required";
                return View();
            }

            username = username.Trim();
            password = password.Trim();

            var exists = _context.Users.Any(u => u.Username.ToLower() == username.ToLower());

            if (exists)
            {
                ViewBag.Error = "Username already exists";
                return View();
            }

            var user = new User
            {
                Username = username,
                Password = password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            System.Console.WriteLine("DATA SAVED");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}