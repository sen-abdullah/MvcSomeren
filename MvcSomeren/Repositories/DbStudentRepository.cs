using Microsoft.Data.SqlClient;
using MvcSomeren.Models;
using System.Collections.Generic;

namespace MvcSomeren.Repositories;

public class DbStudentRepository : IStudentRapository
{
    private readonly string? _connectionString;

    public DbStudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("appsomeren");
    }

    public List<Student> GetAll()
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass FROM Students";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student student = ReadUser(reader);
                students.Add(student); 
            }

            reader.Close();
        }

        return students;
    }

    private Student ReadUser(SqlDataReader reader)
    {
        //retrieve data from fields
        int id = (int)reader["StudentNumber"];
        string name = (string)reader["StudentFirstName"];
        string mobileNumber = (string)reader["StudentLastName"];
        string emailAddress = (string)reader[""];
        string password = (string)reader["Password"];

        return new Student(id, name, mobileNumber, emailAddress, password);
    }
}