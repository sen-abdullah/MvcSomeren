using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class LecturerSupervisorController : Controller
{
    private readonly ILecturerSupervisorRepository _lecturerSupervisorRepository;

    public LecturerSupervisorController(ILecturerSupervisorRepository lecturerSupervisorRepository)
    {
        _lecturerSupervisorRepository = lecturerSupervisorRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_lecturerSupervisorRepository.GetAll());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(_lecturerSupervisorRepository.GetLecturersAndActivities());
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        LecturerSupervisorViewModel lecturerSupervisorViewModel = _lecturerSupervisorRepository.GetSupervisorById((int)id);
        return View(lecturerSupervisorViewModel);
    }

    [HttpPost]
    public IActionResult Edit(LecturerSupervisorViewModel lecturerSupervisorViewModel)
    {
        try
        {
            _lecturerSupervisorRepository.UpdateSupervisor(lecturerSupervisorViewModel);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            LecturerSupervisorViewModel model = _lecturerSupervisorRepository.GetAll();
            model.Supervisor = lecturerSupervisorViewModel.Supervisor;
            return View(model);
        }
    }
}
