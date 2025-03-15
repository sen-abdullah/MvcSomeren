using Microsoft.AspNetCore.Mvc;

namespace MvcSomeren.Controllers;

public class RoomsController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}