using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbDrinksRepository : IDrinksRepository
{
    private readonly string _connectionString;
    private const string CONNECTION_STRING_KEY = "appsomeren";

    public DbDrinksRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
    }


    public List<Drink> GetAll()
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

    public Drink? GetById(int id)
    {
        Drink? drink = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT DrinkId, DrinkName, IsAlcoholicDrink, StockAmountOfDrinks FROM Drink WHERE DrinkId = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

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

    public void Add(Drink drink)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO Drink (DrinkName, IsAlcoholicDrink, StockAmountOfDrinks)" +
                $"VALUES (@DrinkName, @IsAlcoholicDrink, @StockAmountOfDrinks); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkName", drink.Name);
            command.Parameters.AddWithValue("@IsAlcoholicDrink", drink.IsAlcoholic);
            command.Parameters.AddWithValue("@StockAmountOfDrinks", drink.AmountOfStock);
            
            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Adding a new drink failed.");
        }
    }

    public void Update(Drink drink)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE Drink SET DrinkName = @DrinkName, IsAlcoholicDrink = @IsAlcoholicDrink, StockAmountOfDrinks = @StockAmountOfDrinks " +
                $"WHERE DrinkId = @Id; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkName", drink.Name);
            command.Parameters.AddWithValue("@IsAlcoholicDrink", drink.IsAlcoholic);
            command.Parameters.AddWithValue("@StockAmountOfDrinks", drink.AmountOfStock);
            command.Parameters.AddWithValue("@Id", drink.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Drink was not updated.");
        }
    }

    public void Delete(Drink drink)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Drink WHERE DrinkId = @Id;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", drink.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Drink was not deleted.");
        }
    }

    public bool IsDrinkExist(Drink drink)
    {
        List<Drink> drinks = new List<Drink>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT DrinkId, DrinkName, IsAlcoholicDrink, StockAmountOfDrinks FROM Drink WHERE DrinkName = @DrinkName;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DrinkName", drink.Name.Trim());

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Drink drinkItem = ReadDrink(reader);
                drinks.Add(drinkItem);
            }

            reader.Close();
        }

        return drinks.Count > 0;
    }


    private Drink ReadDrink(SqlDataReader reader)
    {
        int id = (int)reader["DrinkId"];
        string name = (string)reader["DrinkName"];
        bool isAlcoholic = (bool)reader["IsAlcoholicDrink"];
        int amountOfStock = (int)reader["StockAmountOfDrinks"];

        return new Drink(id, name, isAlcoholic, amountOfStock);
    }
}