using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthController : Controller
{
    public IActionResult SignIn()
    {
        return View("~/Areas/Admin/Views/Pages/Auth/SignIn.cshtml");
    }

    public IActionResult signUp()
    {
        return View("~/Views/Pages/Auth/SignUp.cshtml");
    }

    public IActionResult resetPassword()
    {
        return View("~/Views/Pages/Auth/ResetPassword.cshtml");
    }

    public IActionResult newPassword()
    {
        return View("~/Views/Pages/Auth/NewPassword.cshtml");
    }
}
