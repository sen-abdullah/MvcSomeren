using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class DrinkOrderController : Controller
{
    private readonly IDrinkOrderRepository _drinkOrderRepository;

    public DrinkOrderController(IDrinkOrderRepository drinkOrderRepository)
    {
        _drinkOrderRepository = drinkOrderRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_drinkOrderRepository.GetAll());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(_drinkOrderRepository.GetDrinksAndStudents());
    }

    [HttpPost]
    public IActionResult Create(DrinkOrderViewModel drinkOrderViewModel)
    {
        try
        {
            int stockAmountOfDrink = _drinkOrderRepository.GetDrinkStockAmount(drinkOrderViewModel.Order.DrinkId);
            if (drinkOrderViewModel.Order.Quantity > stockAmountOfDrink)
            {
                ModelState.AddModelError("ValidationError", "There is not enough drinks!");
                DrinkOrderViewModel model = _drinkOrderRepository.GetAll();
                model.Order = drinkOrderViewModel.Order;
                return View(model);
            }

            _drinkOrderRepository.AddOrder(drinkOrderViewModel);
            _drinkOrderRepository.UpdateDrinkStockAmount(drinkOrderViewModel.Order.DrinkId,
                stockAmountOfDrink - drinkOrderViewModel.Order.Quantity);
            
            Student student = _drinkOrderRepository.GetStudentById(drinkOrderViewModel.Order.StudentId);
            Drink drink = _drinkOrderRepository.GetDrinkById(drinkOrderViewModel.Order.DrinkId);
            
            ModelState.AddModelError("OrderAdded", $"{drinkOrderViewModel.Order.Quantity} {drink.Name} has been ordered for {student.StudentNumber} - {student.StudentFirstName} {student.StudentLastName}! You can order new drinks");
            
            
            return View(_drinkOrderRepository.GetDrinksAndStudents());
        }
        catch (Exception e)
        {
            DrinkOrderViewModel model = _drinkOrderRepository.GetAll();
            model.Order = drinkOrderViewModel.Order;
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

        DrinkOrderViewModel? drinkOrderViewModel = _drinkOrderRepository.GetOrderByID((int)id);
        return View(drinkOrderViewModel);
    }
    
    [HttpPost]
    public IActionResult DeleteOrder(int id)
    {
        try
        {
            _drinkOrderRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View(_drinkOrderRepository.GetOrderByID(id));
        }
    }
}