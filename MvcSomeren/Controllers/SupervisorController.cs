using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class SupervisorController : Controller
{
    private readonly ISupervisorRepository _supervisorRepository;

    public SupervisorController(ISupervisorRepository supervisorRepository)
    {
        _supervisorRepository = supervisorRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        //lecturer
        List<Supervisor> supervisors = _supervisorRepository.GetAll();
        return View(supervisors);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Supervisor supervisor)
    {
        try
        {
            if (_supervisorRepository.IsLecturerExist(supervisor))
            {
                ModelState.AddModelError("ValidationError", "Lecturer already exist!");
                return View(supervisor);
            }

            if (!_supervisorRepository.IsRoomExist(supervisor))
            {
                ModelState.AddModelError("ValidationError", "Room id does not exist!");
                return View(supervisor);
            }

            _supervisorRepository.Add(supervisor);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(supervisor);
        }
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
            if (!_supervisorRepository.IsRoomExist(supervisor))
            {
                ModelState.AddModelError("ValidationError", "Room id does not exist!");
                return View(supervisor);
            }
            
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