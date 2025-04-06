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

    [HttpPost]
    public IActionResult Create(LecturerSupervisorViewModel lecturerSupervisorViewModel)
    {
        try
        {
            _lecturerSupervisorRepository.AddSupervisor(lecturerSupervisorViewModel);
            
                Models.Activity activity =
                    _lecturerSupervisorRepository.GetActivityById(lecturerSupervisorViewModel.Supervisor.ActivityId);

            Lecturer lecturer = _lecturerSupervisorRepository.GetLecturerById(lecturerSupervisorViewModel.Supervisor.LecturerId);
            
            ModelState.AddModelError("OrderAdded", $"{lecturerSupervisorViewModel.Supervisor.SupervisingDate} {activity.ActivityName} has been reserved for {lecturer.LastName} - {lecturer.FirstName}! You are now a supervisor!");
            return View(_lecturerSupervisorRepository.GetLecturersAndActivities());
        }
        catch (Exception e)
        {
            LecturerSupervisorViewModel model = _lecturerSupervisorRepository.GetAll();
            model.Supervisor = lecturerSupervisorViewModel.Supervisor;
            return View(model);
        }
    }
    
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        LecturerSupervisorViewModel? lecturerSupervisorViewModel = _lecturerSupervisorRepository.GetSupervisorById((int)id);
        return View(lecturerSupervisorViewModel);
    }
    
    [HttpPost]
    public IActionResult DeleteSupervisor(int id)
    {
        try
        {
            _lecturerSupervisorRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(_lecturerSupervisorRepository.GetSupervisorById(id));
        }
    }
}
