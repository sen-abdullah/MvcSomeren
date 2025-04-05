using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IDrinksRepository
{
    List<Drink> GetAll();
    
    Drink? GetById(int id);
    
    void Add(Drink drink);
    
    void Update(Drink drink);
    
    void Delete(Drink drink);
    
    bool IsDrinkExist(Drink drink);
    
    List<Drink> Filter(string drinkName);
    
}