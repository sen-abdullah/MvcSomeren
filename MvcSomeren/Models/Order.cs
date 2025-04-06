namespace MvcSomeren.Models
{
    public class Order
    {
        public Order()
        {
            Id = 0;
            DrinkId = 0;
            StudentId = 0;
            Quantity = 0;
        }

        public Order(int id, int drinkId, int studentId, int quantity)
        {
            Id = id;
            DrinkId = drinkId;
            StudentId = studentId;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int DrinkId { get; set; }
        public int StudentId { get; set; }
        public int Quantity { get; set; }
    }
}