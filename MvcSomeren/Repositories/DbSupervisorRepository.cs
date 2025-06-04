using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbSupervisorRepository : ISupervisorRepository
{
    private const string ConnectionStringKey = "appsomeren";
    private readonly string? _connectionString;

    public DbSupervisorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey);
    }
    
    public List<Supervisor> GetAll()
    {
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT SupervisorId, SupervisingDate, LecturerId, ActivityId From Supervisor";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Supervisor supervisor;

            while (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
                supervisors.Add(supervisor);
            }
            reader.Close();
        }
        return supervisors;
    }
    
    public List<Supervisor> GetAllSupervisorsForActivities(int activityId)
    {
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT SupervisorId, SupervisingDate, LecturerId, ActivityId From Supervisor WHERE ActivityId = @ActivityId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Supervisor supervisor;

            while (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
                FillInSupervisor(supervisor);
                supervisors.Add(supervisor);
            }
            reader.Close();
        }
        return supervisors;
    }
    
    /*
    public List<Supervisor> GetAllSupervisorsWithoutActivities(int activityId)
    {
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT SupervisorId, SupervisingDate, LecturerId, ActivityId From Supervisor WHERE LecturerId NOT IN (SELECT LecturerId FROM Supervisor WHERE ActivityId = @ActivityId)";
            //string query = "SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId FROM Lecturer WHERE LecturerId NOT IN (SELECT LecturerId FROM Supervisor WHERE ActivityId = @ActivityId)";

            //string query =
                //"SELECT s.SupervisorId, s.SupervisingDate, s.LecturerId, s.ActivityId FROM Supervisor s WHERE NOT EXISTS (SELECT 1 FROM Supervisor s2  WHERE s2.ActivityId = @ActivityId AND s2.LecturerId = s.LecturerId)";
            
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Supervisor supervisor;

            while (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
                FillInSupervisor(supervisor);
                supervisors.Add(supervisor);
            }
            reader.Close();
        }
        return supervisors;
    }
    */
    
    public List<Lecturer> GetAllLecturersNotSupervisingActivity(int activityId)
    {
        List<Lecturer> lecturers = new List<Lecturer>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT LecturerId, LecturerFirstName, LecturerLastName, 
                                LecturerPhoneNumber, LecturerAge, RoomId 
                         FROM Lecturer 
                         WHERE LecturerId NOT IN (
                             SELECT LecturerId FROM Supervisor WHERE ActivityId = @ActivityId
                         )";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Lecturer lecturer = ReadLecturer(reader); // Make sure you implement this
                lecturers.Add(lecturer);
            }
        }

        return lecturers;
    }
    public Supervisor? GetById(int id)
    {
        Supervisor? supervisor = null;
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT SupervisorId, SupervisingDate, LecturerId, ActivityId From Supervisor WHERE SupervisorId = @SupervisorId";
            SqlCommand com = new SqlCommand(query, conn);
            
            com.Parameters.AddWithValue("@SupervisorId", id);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
                FillInSupervisor(supervisor);
            }
            reader.Close();
        }

        return supervisor;
    }
    
    public void AddSupervisor(Supervisor supervisor, int activityId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    $"INSERT INTO Supervisor (SupervisorId, SupervisingDate, LecturerId, ActivityId)" +
                    $"VALUES (@SupervisorId, @SupervisingDate, @LecturerId, @ActivityId); "+
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                SqlCommand command = new SqlCommand(query, connection);
            
                if (activityId == supervisor.ActivityId)
                {
                    command.Parameters.AddWithValue("@SupervisorId", supervisor.SupervisorId);
                    command.Parameters.AddWithValue("@SupervisingDate",supervisor.SupervisingDate);
                    command.Parameters.AddWithValue("@LecturerId", supervisor.LecturerId);
                    command.Parameters.AddWithValue("@ActivityId", supervisor.ActivityId);
                }

                command.Connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected != 1) throw new Exception("Adding a new Student failed.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private Supervisor ReadSupervisor(SqlDataReader reader)
    {
        return new Supervisor((int)reader["SupervisorId"], (int)reader["SupervisingDate"], (int)reader["LecturerId"], (int)reader["ActivityId"]);
    }
    private Lecturer ReadLecturer(SqlDataReader reader)
    {
        int id = (int)reader["LecturerId"];
        string firstName = (string)reader["LecturerFirstName"];
        string lastName = (string)reader["LecturerLastName"];
        string phoneNumber = (string)reader["LecturerPhoneNumber"];
        int age = (int)reader["LecturerAge"];
        int roomId = (int)reader["RoomId"];

        return new Lecturer(id, firstName, lastName, phoneNumber, age, roomId);
    }
    private void FillInSupervisor(Supervisor supervisor)
    {
        
        if (supervisor == null)
            throw new ArgumentNullException(nameof(supervisor), "Supervisor is null");

        if (CommonRepository._lecturersRepository == null)
            throw new InvalidOperationException("Lecturers repository is not initialized");

        var lecturer = CommonRepository._lecturersRepository.GetById(supervisor.LecturerId);
        if (lecturer == null)
            throw new Exception($"No lecturer found with ID {supervisor.LecturerId}");

        supervisor.Lecturer = lecturer;
        
        supervisor.Lecturer = CommonRepository._lecturersRepository.GetById(supervisor.LecturerId);
    }
}