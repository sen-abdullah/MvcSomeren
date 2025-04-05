using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class DrinksController : Controller
{
    private readonly IDrinksRepository _drinksRepository;

    public DrinksController(IDrinksRepository drinksRepository)
    {
        _drinksRepository = drinksRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Drink> drinks = _drinksRepository.GetAll();
        return View(drinks);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(Drink drink)
    {
        try
        {
            if (_drinksRepository.IsDrinkExist(drink))
            {
                ModelState.AddModelError("ValidationError", "Drink is already exist!");
                return View(drink);
            }

            _drinksRepository.Add(drink);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(drink);
        }
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Drink? drink = _drinksRepository.GetById((int)id);
        return View(drink);
    }

    [HttpPost]
    public IActionResult Edit(Drink drink)
    {
        try
        {
            _drinksRepository.Update(drink);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(drink);
        }
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Drink? drink = _drinksRepository.GetById((int)id);
        return View(drink);
    }

    [HttpPost]
    public IActionResult Delete(Drink drink)
    {
        try
        {
            _drinksRepository.Delete(drink);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(drink);
        }
    }
    
    public IActionResult Filter(String drinkName)
    {
        try
        {
            List<Drink> drinks = _drinksRepository.Filter(drinkName);
            return View(nameof(Index), drinks);
        }
        catch (Exception e)
        {
            List<Drink> drinks = _drinksRepository.GetAll();
            return View(nameof(Index), drinks);
        }
    }
}