using System.Collections.Generic;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IDrinkOrderRepository
{
    DrinkOrderViewModel GetAll();
    DrinkOrderViewModel GetDrinksAndStudents();
    void AddOrder(DrinkOrderViewModel drinkOrderViewModel);
    int GetDrinkStockAmount(int drinkId);
    void UpdateDrinkStockAmount(int drinkId, int stockAmount);
    
    void Delete(int orderId);
    
    DrinkOrderViewModel GetOrderByID(int orderId);
    
    Student? GetStudentById(int id);
    
    Drink? GetDrinkById(int id);
}