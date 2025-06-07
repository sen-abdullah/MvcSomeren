using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class ActivitiesController : Controller
{
    private readonly IActivitiesRepository _activityRepository;
    private readonly ISupervisorRepository _supervisorRepository;
    private readonly ILecturersRepository _lecturersRepository;
    private readonly IParticipantsRepository _participantRepository;
    public ActivitiesController(IActivitiesRepository activityRepository, ISupervisorRepository supervisorRepository, ILecturersRepository lecturersRepository, IParticipantsRepository participantsRepository)
    {
        _activityRepository = activityRepository;
        _supervisorRepository = supervisorRepository;
        _lecturersRepository = lecturersRepository;
        _participantRepository = participantsRepository;
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

    public IActionResult ManageParticipants(int id)
    {
        try
        {
            ManageActivityViewModel model = new ManageActivityViewModel();
            model.ActivityID = id;

            Activity activity = _activityRepository.GetById(id);
            model.Activity = activity;

            model.Participators = CommonRepository._participantsRepository.GetAllParticipantsForActivities(id);
            model.Students = CommonRepository._participantsRepository.GetAllStudentsWithoutActivity(id);

            return View(model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    [HttpPost]
    public IActionResult AddParticipant(int studentId, int activityId)
    {
        try
        {
            CommonRepository._participantsRepository.AddStudent(studentId, activityId);
            var student = _participantRepository.GetById(studentId);

            TempData["ConfirmationMessage"] = $"Student {student?.Student.StudentFirstName} {student?.Student.StudentLastName} was successfully added as participant.";
            return RedirectToAction("ManageParticipants", new { id = activityId });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message; 
            return RedirectToAction("ManageParticipants", new { id = activityId });
        }
    }

    [HttpPost]
    public IActionResult RemoveParticipant(int participantId, int activityId)
    {
        try
        {
            var participant = _participantRepository.GetById(participantId);

            TempData["ConfirmationMessage"] = $"Participant {participant?.Student.StudentFirstName} {participant?.Student.StudentLastName} was successfully removed.";

            CommonRepository._participantsRepository.DeleteParticipant(participantId, activityId);

            return RedirectToAction("ManageParticipants", new { id = activityId });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("ManageParticipants", new { id = activityId });
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