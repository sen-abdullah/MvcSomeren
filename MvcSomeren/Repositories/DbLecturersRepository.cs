using System;
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
            string query =
                "SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge FROM Lecturer ORDER BY LecturerLastName ASC";
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

    public Lecturer? GetById(int id)
    {
        Lecturer? lecturer = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge FROM Lecturer WHERE LecturerId = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                lecturer = ReadLecturer(reader);
            }

            reader.Close();
        }

        return lecturer;
    }


    public void Add(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO Lecturer (LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge)" +
                $"VALUES (@FirstName, @LastName, @PhoneNumber, @Age); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
            command.Parameters.AddWithValue("@LastName", lecturer.LastName);
            command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
            command.Parameters.AddWithValue("@Age", lecturer.Age);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Adding a new Lecturer failed.");
        }
    }

    public void Update(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE Lecturer SET LecturerFirstName = @FirstName, LecturerLastName = @LastName, LecturerPhoneNumber = @PhoneNumber, LecturerAge = @Age " +
                $"WHERE LecturerId = @Id; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
            command.Parameters.AddWithValue("@LastName", lecturer.LastName);
            command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
            command.Parameters.AddWithValue("@Age", lecturer.Age);
            command.Parameters.AddWithValue("@Id", lecturer.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not updated.");
        }
    }

    public void Delete(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Lecturer WHERE LecturerId = @Id;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", lecturer.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not deleted.");
        }
    }


    private Lecturer ReadLecturer(SqlDataReader reader)
    {
        int id = (int)reader["LecturerId"];
        string firstName = (string)reader["LecturerFirstName"];
        string lastName = (string)reader["LecturerLastName"];
        string phoneNumber = (string)reader["LecturerPhoneNumber"];
        int age = (int)reader["LecturerAge"];

        return new Lecturer(id, firstName, lastName, phoneNumber, age);
    }
}