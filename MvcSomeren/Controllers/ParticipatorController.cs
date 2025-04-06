using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class ParticipatorController : Controller
{
    private readonly IParticipatorsRepository _participatorRepository;

    public ParticipatorController(IParticipatorsRepository participatorRepository)
    {
        _participatorRepository = participatorRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Participator> participators = _participatorRepository.GetAll();
        return View(participators);
    }

    [HttpGet]
    public IActionResult Create()
    {
        /*var participators = _participatorRepository.GetAll();

        ViewData["Particapors"] = new SelectList(participators, "StudentId", "StudentId");
        ViewData["Activities"] = new SelectList(participators, "ActivityId", "ActivityId");*/

        return View();
    }

    [HttpPost]
    public IActionResult Create(Participator participator)
    {
        try
        {
            _participatorRepository.Add(participator);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(participator);
        }
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Participator? participator = _participatorRepository.GetById((int)id);
        return View(participator);
    }

    [HttpPost]
    public IActionResult Edit(Participator participator)
    {
        try
        {
            _participatorRepository.Update(participator);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(participator);
        }
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Participator? participator = _participatorRepository.GetById((int)id);
        return View(participator);
    }

    [HttpPost]
    public IActionResult Delete(Participator participator)
    {
        try
        {
            _participatorRepository.Delete(participator);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(participator);
        }
    }
}