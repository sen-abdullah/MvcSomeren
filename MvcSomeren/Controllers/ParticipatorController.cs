using Microsoft.AspNetCore.Mvc;
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
        //lecturer
        List<Participator> participators = _participatorRepository.GetAll();
        return View(participators);
    }

    [HttpGet]
    public IActionResult Create()
    {
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

    public IActionResult Filter(int participatorId)
    {
        try
        {
            List<Participator> participators = _participatorRepository.Filter(participatorId);
            return View(nameof(Index), participators);
        }
        catch (Exception e)
        {
            List<Participator> participators = _participatorRepository.GetAll();
            return View(nameof(Index), participators);
        }
    }
}