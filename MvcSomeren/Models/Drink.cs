namespace MvcSomeren.Models
{
    public class Drink
    {
        public Drink()
        {
            Id = 0;
            Name = "";
            IsAlcoholic = false;
            AmountOfStock = 0;
            VatRate = 0;
        }

        public Drink(int id, string name, bool isAlcoholic, int amountOfStock)
        {
            Id = id;
            Name = name;
            IsAlcoholic = isAlcoholic;
            AmountOfStock = amountOfStock;
            if (isAlcoholic)
            {
                VatRate = 21;
            }
            else
            {
                VatRate = 9;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlcoholic { get; set; }
        public int AmountOfStock { get; set; }
        public int VatRate { get; set; }
    }
}