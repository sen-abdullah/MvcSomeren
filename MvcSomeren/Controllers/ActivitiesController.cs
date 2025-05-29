using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class ActivitiesController : Controller
{
    private readonly IActivitiesRepository _activityRepository;

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
            model.Supervisors = CommonRepository._supervisorRepository.GetAllSupervisorsForActivities(id);
            return View(model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public IActionResult AssignParticipant(int id)
    {
        try
        {
            ManageActivityViewModel model = new ManageActivityViewModel();
            model.ActivityID = id;
            model.Participators = CommonRepository._participantRepository.GetAllParticipantsForActivities(id);
            return View(model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    [HttpGet]
    public IActionResult AssignStudents(int id)
    {
        try
        {
            ViewBag.Activity = _activityRepository.GetById(id);
            ViewBag.Students = CommonRepository._studentRapository.GetAll();
            ViewBag.ActivityId = id;
            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    [HttpPost]
    public IActionResult AssignStudents(int activityId, int studentId)
    {
        try
        {
            var participator = new Participator
            {
                ActivityId = activityId,
                StudentId = studentId,
                ParticipateDate = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day
            };

            CommonRepository._manageParticipantsRepository.Add(participator);
            
            return RedirectToAction("Index", "ManageParticipants");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction("AssignStudents", new { id = activityId });
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