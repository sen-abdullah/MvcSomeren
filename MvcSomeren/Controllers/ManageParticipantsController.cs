using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers
{
    public class ManageParticipantsController : Controller
    {
        private readonly IManageParticipantsRepository _manageParticipantsRepository;

        public ManageParticipantsController(IManageParticipantsRepository manageParticipantsRepository)
        {
            _manageParticipantsRepository = manageParticipantsRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_manageParticipantsRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(_manageParticipantsRepository.GetStudentsAndActivities());
        }

        [HttpPost]
        public IActionResult Create(ManageParticipantViewModel manageParticipantViewModel)
        {
            try
            {
                _manageParticipantsRepository.AddParticipator(manageParticipantViewModel);

                Student student = _manageParticipantsRepository.GetStudentById(manageParticipantViewModel.Participator.StudentId);
                Activity activity = _manageParticipantsRepository.GetActivityById(manageParticipantViewModel.Participator.ActivityId);

                ModelState.AddModelError("ParticipatorAdded", $"{manageParticipantViewModel.Participator.ParticipateDate} {activity.ActivityName} has been assigned for {student.StudentNumber} - {student.StudentFirstName} {student.StudentLastName}! You can choose new Participators");


                return View(_manageParticipantsRepository.GetStudentsAndActivities());
            }
            catch (Exception e)
            {
                ManageParticipantViewModel model = _manageParticipantsRepository.GetAll();
                model.Participator = manageParticipantViewModel.Participator;
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

            ManageParticipantViewModel? manageParticipantViewModel = _manageParticipantsRepository.GetParticipatorByID((int)id);
            return View(manageParticipantViewModel);
        }

        [HttpPost]
        public IActionResult DeleteParticipant(int id)
        {
            try
            {
                _manageParticipantsRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View(_manageParticipantsRepository.GetParticipatorByID(id));
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ManageParticipantViewModel manageParticipantViewModel = _manageParticipantsRepository.GetParticipatorByID((int)id);
            return View(manageParticipantViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ManageParticipantViewModel manageParticipantViewModel)
        {
            try
            {
                _manageParticipantsRepository.UpdateParticipator(manageParticipantViewModel);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ManageParticipantViewModel model = _manageParticipantsRepository.GetAll();
                model.Participator = manageParticipantViewModel.Participator;
                return View(model);
            }
        }
    }
}
