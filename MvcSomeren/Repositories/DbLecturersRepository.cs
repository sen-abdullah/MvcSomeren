using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbLecturersRepository : ILecturersRepository
{
    private readonly string? _connectionString;

    public DbLecturersRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("appsomeren");
    }

    public List<Lecturer> GetAll()
    {
        List<Lecturer> lecturers = new List<Lecturer>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT LecturerID, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge FROM Lecturers";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Lecturer lecturer = ReadLecturer(reader);
                lecturers.Add(lecturer);
            }

            reader.Close();
        }

        return lecturers;
    }
    
    private Lecturer ReadLecturer(SqlDataReader reader)
    {
        int id = (int)reader["LecturerID"];
        string firstName = (string)reader["LecturerFirstName"];
        string lastName = (string)reader["LecturerLastName"];
        string phoneNumber = (string)reader["LecturerPhoneNumber"];
        int age = (int)reader["LecturerAge"];

        return new Lecturer(id, firstName, lastName, phoneNumber, age);
    }
}