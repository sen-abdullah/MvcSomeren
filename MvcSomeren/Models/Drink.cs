namespace MvcSomeren.Models
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        public int AmountOfStock { get; set; }
        
        public Drink()
        {
            Id = 0;
            Name = "";
            IsAlcoholic = false;
            AmountOfStock = 0;
        }

        public Drink(int id, string name, bool isAlcoholic, int amountOfStock)
        {
            Id = id;
            Name = name;
            IsAlcoholic = isAlcoholic;
            AmountOfStock = amountOfStock;
        }
    }
}