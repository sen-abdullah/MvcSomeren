using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class SupervisorsController : Controller
{
    private readonly ISupervisorRepository _supervisorRepository;

    public SupervisorsController(ISupervisorRepository supervisorRepository)
    {
        _supervisorRepository = supervisorRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Supervisor> supervisors = _supervisorRepository.GetAll();
        return View(supervisors);
    }

    [HttpGet]
    public IActionResult Create()
    {
        // Assuming you have a Supervisor model and database context
        var supervisors = _supervisorRepository.GetAll();

        // Prepare the SelectList for LecturerId dropdown
        ViewData["Supervisors"] = new SelectList(supervisors, "LecturerId", "LastName");

        // Prepare the SelectList for SupervisingDate dropdown
        ViewData["Activities"] = new SelectList(supervisors, "ActivityId", "ActivityId");

        return View();
    }

    [HttpPost]
    public IActionResult Create(Supervisor supervisor)
    {
        if (ModelState.IsValid)
        {
            _supervisorRepository.Add(supervisor);
            return RedirectToAction(nameof(Index)); // Or any other action you want after submission
        }

        return View(supervisor);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Supervisor? supervisor = _supervisorRepository.GetById((int)id);
        return View(supervisor);
    }

    [HttpPost]
    public IActionResult Edit(Supervisor supervisor)
    {
        try
        {
            _supervisorRepository.Update(supervisor);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(supervisor);
        }
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Supervisor? supervisor = _supervisorRepository.GetById((int)id);
        return View(supervisor);
    }

    [HttpPost]
    public IActionResult Delete(Supervisor supervisor)
    {
        try
        {
            _supervisorRepository.Delete(supervisor);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(supervisor);
        }
    }

    public IActionResult Filter(String lastName)
    {
        try
        {
            List<Supervisor> supervisors = _supervisorRepository.Filter(lastName);
            return View(nameof(Index), supervisors);
        }
        catch (Exception e)
        {
            List<Supervisor> supervisors = _supervisorRepository.GetAll();
            return View(nameof(Index), supervisors);
        }
    }
} 