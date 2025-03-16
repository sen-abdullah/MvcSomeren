using Microsoft.AspNetCore.Mvc;

namespace MvcSomeren.Controllers;

public class LecturersController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}