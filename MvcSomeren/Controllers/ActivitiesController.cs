using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class ActivitiesController : Controller
{
    private readonly IActivitiesRepository _activityRepository;
    private readonly ISupervisorRepository _supervisorRepository;

    public ActivitiesController(IActivitiesRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public IActionResult Index()
    {
        List<Activity> activities = _activityRepository.GetAll();
        return View(activities);
    }
    public IActionResult Manage(int id)
    {
        try
        {
            ManageActivityViewModel model = new ManageActivityViewModel();
            model.ActivityID = id;
            
            Activity activity = _activityRepository.GetById(id);
            model.Activity = activity;
            
            model.Supervisors = CommonRepository._supervisorRepository.GetAllSupervisorsForActivities(id);
            model.Lecturers = CommonRepository._supervisorRepository.GetAllLecturersNotSupervisingActivity(id);
            //model.NonSupervisor = CommonRepository._supervisorRepository.GetAllSupervisorsWithoutActivities(id);
            return View(model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    /*
    [HttpGet]
    public IActionResult DeleteSupervisor(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        LecturerSupervisorViewModel? lecturerSupervisorViewModel = CommonRepository._lecturerSupervisorRepository.GetSupervisorById((int)id);
        return View(lecturerSupervisorViewModel);
    }
    */

    [HttpPost]
    public IActionResult DeleteSupervisor(int supervisorId, int activityId)
    {
        try
        {
            CommonRepository._lecturerSupervisorRepository.Delete(supervisorId, activityId);
            return RedirectToAction("Manage" ,new {id = activityId});
        }
        catch (Exception e)
        {
            //ViewBag.ErrorMessage = "Could Not Add Supervisors"; 
            return View(e.Message);	
        }
    }
    
    /*
    [HttpPost]
    public ActionResult AssignSupervisor(Supervisor supervisor)
    {
        try
        {
            _supervisorRepository.AddSupervisor(supervisor, supervisor.ActivityId);
            return RedirectToAction("Manage", new { id = supervisor.ActivityId });
            
            //Activity activity = _activityRepository.GetById(activityID);
            //model.Activity = activity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(supervisor);
        }
    }
    */

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Activity activity)
    {
        try
        {
            _activityRepository.Add(activity);

            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(activity);
        }
    }

    [HttpGet("Activities/Edit/{ActivityId}")]
    public ActionResult Edit(int? activityId)
    {
        if (activityId == null)
        {
            return NotFound();
        }

        Activity? activity = _activityRepository.GetById((int)activityId);
        return View(activity);
    }

    [HttpPost]
    public ActionResult Edit(Activity activity)
    {
        try
        {
            _activityRepository.Update(activity);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(activity);
        }
    }

    [HttpGet("Activities/Delete/{ActivityId}")]
    public ActionResult Delete(int? activityId)
    {
        if (activityId == null)
        {
            return NotFound();
        }

        Activity? activity = _activityRepository.GetById((int)activityId);
        return View(activity);
    }

    [HttpPost]
    public ActionResult Delete(Activity activity)
    {
        try
        {
            _activityRepository.Delete(activity);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(activity);
        }
    }

    public IActionResult Filter(string activityName)
    {
        try
        {
            List<Activity> activities = _activityRepository.Filter(activityName);
            return View(nameof(Index), activities);
        }
        catch (Exception e)
        {
            List<Activity> activities = _activityRepository.GetAll();
            return View(nameof(Index), activities);
        }
    }
}