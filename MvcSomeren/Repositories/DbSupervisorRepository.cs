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
            string query = "SELECT SupervisorId, SupervisingDate, LecturerId, ActivityId From Supervisor WHERE ActivityId = @ActivityId ORDER BY SupervisingDate ASC";
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
                         ) ORDER BY LecturerLastName ASC";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Lecturer lecturer = ReadLecturer(reader);
                lecturers.Add(lecturer);
            }
        }

        return lecturers;
    }
    
    public void DeleteSupervisor(int supervisorId, int activityId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Supervisor WHERE SupervisorId = @Id AND ActivityId = @ActivityId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", supervisorId);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            try
            {
                connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();

                if (numberOfRowsAffected != 1)
                {
                    throw new Exception($"Delete failed. Rows affected: {numberOfRowsAffected}. SupervisorId: {supervisorId}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Supervisor: " + ex.Message, ex);
            }
        }
    }
    
    public void AddSupervisor(Supervisor supervisor, int activityId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    $"INSERT INTO Supervisor (SupervisingDate, LecturerId, ActivityId) " +
                    $"VALUES (@SupervisingDate, @LecturerId, @ActivityId); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int);";

                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@SupervisingDate",supervisor.SupervisingDate);
                command.Parameters.AddWithValue("@LecturerId", supervisor.LecturerId);
                command.Parameters.AddWithValue("@ActivityId", activityId);

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