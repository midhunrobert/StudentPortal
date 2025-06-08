using Microsoft.AspNetCore.Mvc;
using StudentPortal.Models.DTOs.Auth;

public class AccountController : Controller
{
    


    


    

    [HttpGet]
    public IActionResult FacultyRegister() => View(new RegisterDto());


    


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
