using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentRapository _studentRapository;

    public StudentsController(IStudentRapository studentRapository)
    {
        _studentRapository = studentRapository;
    }

    public IActionResult Index()
    {
        List<Student> students = _studentRapository.GetAll();
        return View(students);
    }
}

