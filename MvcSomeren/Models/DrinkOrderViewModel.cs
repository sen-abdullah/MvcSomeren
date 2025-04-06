namespace MvcSomeren.Models;

public class DrinkOrderViewModel
{
    public DrinkOrderViewModel()
    {
        Drinks = new List<Drink>();
        Students = new List<Student>();
        Quantities = new List<int>();
        Order = null;
        AllStudents = new List<Student>();
        AllDrinks = new List<Drink>();
        AllOrders = new List<Order>();
    }

    public DrinkOrderViewModel(List<Drink> drinks, List<Student> students, List<int> quantities, Order order, List<Student> allStudents, List<Drink> allDrinks, List<Order> allOrders)
    {
        Drinks = drinks;
        Students = students;
        Quantities = quantities;
        Order = order;
        AllStudents = allStudents;
        AllDrinks = allDrinks;
        AllOrders = allOrders;
    }

    public List<Drink> Drinks { get; set; }
    public List<Student> Students { get; set; }
    public List<int> Quantities { get; set; }
    public Order? Order { get; set; }
    public List<Drink> AllDrinks { get; set; }
    public List<Student> AllStudents { get; set; }
    public List<Order> AllOrders { get; set; }
}