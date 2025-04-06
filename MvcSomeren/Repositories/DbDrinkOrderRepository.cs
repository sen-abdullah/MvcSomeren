using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbDrinkOrderRepository : IDrinkOrderRepository
{
    private const string ConnectionStringKey = "appsomeren";
    private readonly string? _connectionString;

    public DbDrinkOrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey);
    }


    //It fetches all the orders
    public DrinkOrderViewModel GetAll()
    {
        List<Drink> drinks = new List<Drink>();
        List<Student> students = new List<Student>();
        List<int> quantities = new List<int>();
        List<Order> orders = new List<Order>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT [Order].OrderId, [Order].Quantity, Student.StudentId, Student.StudentNumber, Student.StudentFirstName, Student.StudentLastName, Student.StudentClass, Student.StudentPhonenumber, Student.StudentRoomId, Drink.DrinkId, Drink.DrinkName, Drink.IsAlcoholicDrink, Drink.StockAmountOfDrinks FROM [Order] JOIN Student ON [Order].StudentId = Student.StudentId JOIN Drink ON [Order].DrinkId = Drink.DrinkId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student student = ReadStudent(reader);
                students.Add(student);
                
                Drink drink = ReadDrink(reader);
                drinks.Add(drink);
                
                int quantity = (int)reader["Quantity"];
                quantities.Add(quantity);
                
                int orderId = (int)reader["OrderId"];
                int studentId = (int)reader["StudentId"];
                int drinkId = (int)reader["DrinkId"];
                
                Order order = new Order(orderId, studentId, drinkId, quantity);
                orders.Add(order);
                
            }

            reader.Close();
        }

        return new DrinkOrderViewModel(drinks, students, quantities, null, GetStudents(), GetDrinks(), orders);
    }

    public DrinkOrderViewModel GetDrinksAndStudents()
    {
        return new DrinkOrderViewModel(new List<Drink>(), new List<Student>(), new List<int>(), null, GetStudents(), GetDrinks(), new List<Order>());
    }

    public void AddOrder(DrinkOrderViewModel drinkOrderViewModel)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO [Order] (DrinkId, StudentId, Quantity)" +
                $"VALUES (@DrinkId, @StudentId, @Quantity); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkId", drinkOrderViewModel.Order.DrinkId);
            command.Parameters.AddWithValue("@StudentId", drinkOrderViewModel.Order.StudentId);
            command.Parameters.AddWithValue("@Quantity", drinkOrderViewModel.Order.Quantity);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Adding a new order failed.");
        }
    }

    public int GetDrinkStockAmount(int drinkId)
    {
        int drinkStockAmount = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT StockAmountOfDrinks FROM Drink WHERE DrinkId = @DrinkId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkId", drinkId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                drinkStockAmount = (int)reader["StockAmountOfDrinks"];
            }

            reader.Close();
        }

        return drinkStockAmount;
    }

    public void UpdateDrinkStockAmount(int drinkId, int stockAmount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE Drink SET StockAmountOfDrinks = @StockAmountOfDrinks " +
                $"WHERE DrinkId = @DrinkId; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StockAmountOfDrinks", stockAmount);
            command.Parameters.AddWithValue("@DrinkId", drinkId);
            
            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not updated.");
        }
    }

    public void Delete(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM [Order] WHERE OrderId = @Id;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", orderId);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Order was not deleted.");
        }
    }

    public void UpdateOrder(DrinkOrderViewModel drinkOrderViewModel)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE [Order] SET DrinkId = @DrinkId, StudentId = @StudentId, Quantity = @Quantity " +
                $"WHERE OrderId = @OrderId; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkId", drinkOrderViewModel.Order.DrinkId);
            command.Parameters.AddWithValue("@StudentId", drinkOrderViewModel.Order.StudentId);
            command.Parameters.AddWithValue("@Quantity", drinkOrderViewModel.Order.Quantity);
            command.Parameters.AddWithValue("@OrderId", drinkOrderViewModel.Order.Id);
            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Order was not updated.");
        }
    }

    public DrinkOrderViewModel GetOrderByID(int orderId)
    {
        List<Drink> drinks = new List<Drink>();
        List<Student> students = new List<Student>();
        List<int> quantities = new List<int>();
        Order? order = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT [Order].OrderId, [Order].Quantity, Student.StudentId, Student.StudentNumber, Student.StudentFirstName, Student.StudentLastName, Student.StudentClass, Student.StudentPhonenumber, Student.StudentRoomId, Drink.DrinkId, Drink.DrinkName, Drink.IsAlcoholicDrink, Drink.StockAmountOfDrinks FROM [Order] JOIN Student ON [Order].StudentId = Student.StudentId JOIN Drink ON [Order].DrinkId = Drink.DrinkId WHERE [Order].OrderId = @OrderId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student student = ReadStudent(reader);
                students.Add(student);
                
                Drink drink = ReadDrink(reader);
                drinks.Add(drink);
                
                int quantity = (int)reader["Quantity"];
                quantities.Add(quantity);
                
                
                order = ReadOrder(reader);
            }

            reader.Close();
        }

        return new DrinkOrderViewModel(drinks, students, quantities, order, GetStudents(), GetDrinks(), new List<Order>());
    }

    public Student? GetStudentById(int id)
    {
        Student? student = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT StudentId, StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, StudentRoomId FROM Student WHERE StudentId = @StudentId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentId", id);
            
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            { student = ReadStudent(reader);
                
            }

            reader.Close();
        }
        return student;

    }

    public Drink? GetDrinkById(int id)
    {
        Drink? drink = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT DrinkId, DrinkName, IsAlcoholicDrink, StockAmountOfDrinks FROM Drink WHERE DrinkId = @DrinkId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkId", id);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            { 
                drink = ReadDrink(reader);
            }

            reader.Close();
        }

        return drink;
    }

    public Order? GetOrderById(int id)
    {
        Order? order = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT OrderId, DrinkId, StudentId, Quantity FROM [Order] WHERE OrderId = @OrderId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderId", id);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            { 
                order = ReadOrder(reader);
            }

            reader.Close();
        }

        return order;
    }


    private List<Student> GetStudents()
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT StudentId, StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, StudentRoomId FROM Student";

            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student student = ReadStudent(reader);
                students.Add(student);
            }

            reader.Close();
        }

        return students;
    }

    private List<Drink> GetDrinks()
    {
        List<Drink> drinks = new List<Drink>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT DrinkId, DrinkName, IsAlcoholicDrink, StockAmountOfDrinks FROM Drink";

            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Drink drink = ReadDrink(reader);
                drinks.Add(drink);
            }

            reader.Close();
        }

        return drinks;
    }
    


    private Student ReadStudent(SqlDataReader reader)
    {
        int studentId = (int)reader["StudentId"];
        int studentNumber = (int)reader["StudentNumber"];
        string studentFirstName = (string)reader["StudentFirstName"];
        string studentLastName = (string)reader["StudentLastName"];
        int studentPhoneNumber = (int)reader["StudentPhoneNumber"];
        string studentClass = (string)reader["StudentClass"];
        int studentRoomId = (int)reader["StudentRoomId"];


        return new Student(
            studentId,
            studentNumber,
            studentFirstName,
            studentLastName,
            studentPhoneNumber,
            studentClass,
            studentRoomId
        );
    }

    private Drink ReadDrink(SqlDataReader reader)
    {
        int id = (int)reader["DrinkId"];
        string name = (string)reader["DrinkName"];
        bool isAlcoholic = (bool)reader["IsAlcoholicDrink"];
        int amountOfStock = (int)reader["StockAmountOfDrinks"];

        return new Drink(id, name, isAlcoholic, amountOfStock);
    }

    private Order ReadOrder(SqlDataReader reader)
    {
        int id = (int)reader["OrderId"];
        int studentId = (int)reader["StudentId"];
        int drinkId = (int)reader["DrinkId"];
        int quantity = (int)reader["Quantity"];
        return new Order(id, drinkId, studentId, quantity);
        
    }
}