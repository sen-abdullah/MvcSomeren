using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Repositories;

namespace MvcSomeren.Controllers;

public class DrinkOrderController: Controller
{
    
    private readonly IDrinkOrderRepository _drinkOrderRepository;

    public DrinkOrderController(IDrinkOrderRepository drinkOrderRepository)
    {
        _drinkOrderRepository = drinkOrderRepository;
    }

    
    public IActionResult Index()
    {
        return View(_drinkOrderRepository.Get());
    }
}