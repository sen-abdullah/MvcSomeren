using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbDrinksRepository: IDrinksRepository
{
    
    private readonly string _connectionString;
    private const string CONNECTION_STRING_KEY = "appsomeren";
    
    public DbDrinksRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
    }

    
    public List<Drink> GetAll()
    {
        throw new NotImplementedException();
    }

    public Drink? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Drink drink)
    {
        throw new NotImplementedException();
    }

    public void Update(Drink drink)
    {
        throw new NotImplementedException();
    }

    public void Delete(Drink drink)
    {
        throw new NotImplementedException();
    }
    
    
    private Drink ReadDrink(SqlDataReader reader)
    {
        int id = (int)reader["DrinkId"];
        string name = (string)reader["DrinkName"];
        bool isAlcoholic = (bool)reader["isAlcoholicDrink"];
        string phoneNumber = (string)reader["StockAmountOfDrinks"];
        int age = (int)reader["LecturerAge"];
        int roomId = (int)reader["RoomId"];

        
        return new Drink(drinkId, drinkName, isAlcoholicDrink, stockAmountOfDrinks)
        return new Lecturer(id, firstName, lastName, phoneNumber, age, roomId);
    }

}