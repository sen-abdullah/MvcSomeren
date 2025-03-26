using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentRapository _studentRepository;

    public StudentsController(IStudentRapository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public IActionResult Index()
    {
        List<Student> students = _studentRepository.GetAll();
        return View(students);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Student student)
    {
        try
        {
            if (_studentRepository.IsStudentExist(student))
            {
                ModelState.AddModelError("AlreadyExist", "Student Already Exist");
                return View(student);
            }
            
            if (!_studentRepository.IsRoomIdExist(student))
            {
                ModelState.AddModelError("AlreadyExist", "Room doesn't Already Exist");
                return View(student);
            }
            
            _studentRepository.AddStudent(student);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(student);
        }
    }

    [HttpGet ("Students/Edit/{studentId}")]
    public ActionResult Edit(int? studentId)
    {
        if (studentId == null)
        {
            return NotFound();
        }

        Student? student = _studentRepository.GetById((int)studentId);
        return View(student);
    }

    [HttpPost]
    public ActionResult Edit(Student student)
    {
        try
        {
            _studentRepository.UpdateStudent(student);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(student);
        }
    }

    [HttpGet ("Students/Delete/{studentId}")]
    public ActionResult Delete(int? studentId)
    {
        if (studentId == null)
        {
            return NotFound();
        }

        Student? student = _studentRepository.GetById((int)studentId);
        return View(student);
    }

    [HttpPost]
    public ActionResult Delete(Student student)
    {
        try
        {
            _studentRepository.DeleteStudent(student);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(student);
        }
    }
    
    
    public IActionResult Filter(string lastName)
    {
        try
        {
            List<Student> students = _studentRepository.Filter(lastName);
            return View(nameof(Index), students);
        }
        catch (Exception e)
        {
            List<Student> students = _studentRepository.GetAll();
            return View(nameof(Index), students);
        }
    }
}
