using Microsoft.AspNetCore.Mvc;
using VisitorLog.Models;

namespace VisitorLog.Controllers
{
    public class AccountController : Controller
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";
        private const string SessionKey = "IsAuthenticated";
        private const string SessionUser = "AdminUsername";

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString(SessionKey) == "true")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Username == AdminUsername && model.Password == AdminPassword)
            {
                HttpContext.Session.SetString(SessionKey, "true");
                HttpContext.Session.SetString(SessionUser, model.Username);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
