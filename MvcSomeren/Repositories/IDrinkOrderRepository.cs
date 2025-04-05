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
    
    void UpdateOrder(DrinkOrderViewModel drinkOrderViewModel);
    
    DrinkOrderViewModel GetOrderByID(int orderId);
    
    Student? GetStudentById(int id);
    
    Drink? GetDrinkById(int id);
    
    Order? GetOrderById(int id);
}