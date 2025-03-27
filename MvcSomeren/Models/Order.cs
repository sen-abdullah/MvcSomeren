namespace MvcSomeren.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int DrinkId { get; set; }
        public int StudentId { get; set; }
        public int LecturerId { get; set; }
        public Order()
        {
            OrderId = 0;
            DrinkId = 0;
            StudentId = 0;
            LecturerId = 0;
        }

        public Order(int orderId, int drinkId, int studentId, int lecturerId)
        {
            OrderId = orderId;
            DrinkId = drinkId;
            StudentId = studentId;
            LecturerId = lecturerId;
        }
    }
}
