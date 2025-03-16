using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class LecturersController : Controller
{
    private readonly ILecturersRepository _lecturersRepository;

    public LecturersController(ILecturersRepository lecturersRepository)
    {
        _lecturersRepository = lecturersRepository;
    }

    public IActionResult Index()
    {
        List<Lecturer> lecturers = _lecturersRepository.GetAll();
        return View(lecturers);
    }
}