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