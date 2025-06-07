using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class ActivitiesController : Controller
{
    private readonly IActivitiesRepository _activityRepository;
    private readonly ISupervisorRepository _supervisorRepository;
    private readonly ILecturersRepository _lecturersRepository;

    public ActivitiesController(IActivitiesRepository activityRepository, ISupervisorRepository supervisorRepository, ILecturersRepository lecturersRepository)
    {
        _activityRepository = activityRepository;
        _supervisorRepository = supervisorRepository;
        _lecturersRepository = lecturersRepository;
    }

    public IActionResult Index()
    {
        List<Activity> activities = _activityRepository.GetAll();
        return View(activities);
    }
    public IActionResult ManageSupervisors(int id)
    {
        try
        {
            ManageActivityViewModel model = new ManageActivityViewModel();
            model.ActivityID = id;
            
            Activity activity = _activityRepository.GetById(id);
            model.Activity = activity;
            
            model.Supervisors = _supervisorRepository.GetAllSupervisorsForActivities(id);
            model.Lecturers = _supervisorRepository.GetAllLecturersNotSupervisingActivity(id);
            return View(model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    [HttpPost]
    public IActionResult DeleteSupervisor(int supervisorId, int activityId)
    {
        try
        {
            var supervisor = _supervisorRepository.GetById(supervisorId);
            var lecturer = _lecturersRepository.GetById(supervisor.LecturerId);
            
            _supervisorRepository.DeleteSupervisor(supervisorId, activityId);
            TempData["Success"] = $"Successfully removed supervisor: {lecturer.FirstName} {lecturer.LastName}.";
            return RedirectToAction("ManageSupervisors" ,new {id = activityId});
        }
        catch (Exception e)
        {
            return View(e.Message);	
        }
    }
    
    [HttpPost]
    public IActionResult AddSupervisor(int lecturerId, int activityId)
    {
        try
        {
            int supervisingDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            var supervisor = new Supervisor
            {
                LecturerId = lecturerId,
                ActivityId = activityId,
                SupervisingDate = supervisingDate
            };

            _supervisorRepository.AddSupervisor(supervisor, activityId);
            var lecturer = _lecturersRepository.GetById(supervisor.LecturerId);
            
            TempData["Success"] = $"Successfully added supervisor! {lecturer.FirstName} {lecturer?.LastName}, Supervising Date: {supervisor.SupervisingDate}.";
            return RedirectToAction("ManageSupervisors", new { id = activityId });
        }
        catch (Exception e)
        {
            TempData["Error"] = "Could not add supervisor: " + e.Message;
            return RedirectToAction("ManageSupervisors", new { id = activityId });
        }
    }

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