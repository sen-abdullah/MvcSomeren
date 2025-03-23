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

    [HttpGet]
    public IActionResult Index()
    {
        List<Lecturer> lecturers = _lecturersRepository.GetAll();
        return View(lecturers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Lecturer lecturer)
    {
        try
        {
            if (_lecturersRepository.IsLecturerExist(lecturer))
            {
                ModelState.AddModelError("AlreadyExist", "Lecturer Already Exist");
                return View(lecturer);
            }

            _lecturersRepository.Add(lecturer);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(lecturer);
        }
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Lecturer? lecturer = _lecturersRepository.GetById((int)id);
        return View(lecturer);
    }

    [HttpPost]
    public IActionResult Edit(Lecturer lecturer)
    {
        try
        {
            if (_lecturersRepository.IsLecturerExist(lecturer))
            {
                ModelState.AddModelError("AlreadyExist", "Lecturer Already Exist");
                return View(lecturer);
            }

            _lecturersRepository.Update(lecturer);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(lecturer);
        }
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Lecturer? lecturer = _lecturersRepository.GetById((int)id);
        return View(lecturer);
    }

    [HttpPost]
    public IActionResult Delete(Lecturer lecturer)
    {
        try
        {
            _lecturersRepository.Delete(lecturer);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(lecturer);
        }
    }
}