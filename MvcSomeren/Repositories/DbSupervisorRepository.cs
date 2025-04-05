using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbSupervisorRepository : ISupervisorRepository
{
    private readonly string? _connectionString;
    private const string CONNECTION_STRING_KEY = "appsomeren";

    public DbSupervisorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
    }

    public List<Supervisor> GetAll()
    {
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT L.LecturerId, L.LecturerFirstName, L.LecturerLastName, L.LecturerPhoneNumber, L.LecturerAge, L.RoomId, S.SupervisorId, S.SupervisingDate, S.ActivityId FROM Supervisor AS S JOIN Lecturer AS L ON S.LecturerId = L.LecturerId ";
            
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Supervisor supervisor = ReadSupervisor(reader);
                supervisors.Add(supervisor);
            }

            reader.Close();
        }

        return supervisors;
    }

    public Supervisor? GetById(int id)
    {
        Supervisor? supervisor = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT * FROM Supervisor WHERE SupervisorId = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
            }

            reader.Close();
        }

        return supervisor;
    }


    public void Add(Supervisor supervisor)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO Supervisor (SupervisingDate, LecturerId, ActivityId) VALUES (@SupervisingDate, @LecturerId, @ActivityId); SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupervisingDate", supervisor.SupervisingDate);
            command.Parameters.AddWithValue("@LecturerId", supervisor.LecturerId);
            command.Parameters.AddWithValue("@ActivityId", supervisor.ActivityId);
            
            try
            {
                connection.Open();
                var newSupervisorId = command.ExecuteScalar();
                if (newSupervisorId != null)
                {
                    supervisor.SupervisorId = Convert.ToInt32(newSupervisorId);
                }
                else
                {
                    throw new Exception("Failed to retrieve the newly inserted SupervisorId.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding a new supervisor: " + ex.Message, ex);
            }
        }
    }

    public void Update(Supervisor supervisor)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE Supervisor SET SupervisingDate = @SupervisingDate, LecturerId = @LecturerId, ActivityId = @ActivityId WHERE SupervisorId = @Id; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupervisingDate", supervisor.SupervisingDate);
            command.Parameters.AddWithValue("@LecturerId", supervisor.LecturerId);
            command.Parameters.AddWithValue("@ActivityId", supervisor.ActivityId);
            command.Parameters.AddWithValue("@Id", supervisor.SupervisorId);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not updated.");
        }
    }

    public void Delete(Supervisor supervisor)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Supervisor WHERE SupervisorId = @Id;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", supervisor.SupervisorId);

            try
            {
                connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();

                if (numberOfRowsAffected != 1)
                {
                    throw new Exception($"Delete failed. Rows affected: {numberOfRowsAffected}. SupervisorId: {supervisor.SupervisorId}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Supervisor: " + ex.Message, ex);
            }
        }
    }

    public bool IsSupervisorExist(Supervisor supervisor)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT 1 FROM Supervisor JOIN Lecturer ON Supervisor.LecturerId = Lecturer.LecturerId WHERE LecturerId = @LecturerId; ";
                
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LecturerId", supervisor.LecturerId);

            command.Connection.Open();
            var result = command.ExecuteScalar();
            return result != null;
        }
    }
    

    public List<Supervisor> Filter(string lastName)
    {
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT L.LecturerId, L.LecturerFirstName, L.LecturerLastName, L.LecturerPhoneNumber, L.LecturerAge, L.RoomId, S.SupervisorId, S.SupervisingDate, S.ActivityId FROM Supervisor AS S JOIN Lecturer AS L ON S.LecturerId = L.LecturerId WHERE L.LecturerLastName = @LastName;";
            
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LastName", lastName.Trim() + "%");

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Supervisor supervisor = ReadSupervisor(reader);
                supervisors.Add(supervisor);
            }

            reader.Close();
        }

        return supervisors;
    }
    
    private Supervisor ReadSupervisor(SqlDataReader reader)
    {
        int supervisorId = (int)reader["SupervisorId"];
        int supervisingDate = (int)reader["SupervisingDate"];
        int lecturerId = (int)reader["LecturerId"];
        int activityId = (int)reader["ActivityId"];
        

        return new Supervisor(supervisorId, supervisingDate, lecturerId, activityId);
    }
}