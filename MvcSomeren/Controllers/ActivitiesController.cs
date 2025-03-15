using Microsoft.AspNetCore.Mvc;

namespace MvcSomeren.Controllers;

public class ActivitiesController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}