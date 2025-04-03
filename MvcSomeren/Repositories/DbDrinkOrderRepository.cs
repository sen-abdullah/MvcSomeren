using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbDrinkOrderRepository : IDrinkOrderRepository
{
    private readonly string? _connectionString;
    private const string ConnectionStringKey = "appsomeren";

    public DbDrinkOrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey);
    }


    public DrinkOrderViewModel Get()
    {
        return new DrinkOrderViewModel(getDrinks(), GetStudents());
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
    
    private List<Drink> getDrinks()
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


        return new Student(studentId, studentNumber, studentFirstName, studentLastName, studentPhoneNumber,
            studentClass, 1);
    }
    
    private Drink ReadDrink(SqlDataReader reader)
    {// DrinkName, IsAlcoholicDrink, StockAmountOfDrinks
        int id = (int)reader["DrinkId"];
        string name = (string)reader["DrinkName"];
        bool isAlcoholic = (bool)reader["IsAlcoholicDrink"];
        int amountOfStock = (int)reader["StockAmountOfDrinks"];
        
        return new Drink(id, name, isAlcoholic, amountOfStock);
    }
}