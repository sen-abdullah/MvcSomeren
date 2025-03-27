namespace MvcSomeren.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public bool IsAlcoholicDrink { get; set; }
        public int StockAmountOfDrinks { get; set; }
        public Drink()
        {
            DrinkId = 0;
            DrinkName = "";
            IsAlcoholicDrink = false;
            StockAmountOfDrinks = 0;
        }

        public Drink(int drinkId, string drinkName, bool isAlcoholicDrink, int stockAmountOfDrinks)
        {
            DrinkId = drinkId;
            DrinkName = drinkName;
            IsAlcoholicDrink = isAlcoholicDrink;
            StockAmountOfDrinks = stockAmountOfDrinks;
        }
    }
}
