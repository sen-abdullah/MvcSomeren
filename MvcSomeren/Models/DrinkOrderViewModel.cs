using System.Collections.Generic;

namespace MvcSomeren.Models;

public class DrinkOrderViewModel
{
    public List<Drink> Drinks { get; set; }
    public List<Student> Students { get; set; }

    public DrinkOrderViewModel()
    {
        Drinks = new List<Drink>();
        Students = new List<Student>();
    }

    public DrinkOrderViewModel(List<Drink> drinks, List<Student> students)
    {
        Drinks = drinks;
        Students = students;
    }
}