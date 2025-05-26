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
    private void FillInSupervisor(Supervisor supervisor)
    {
        supervisor.Lecturer = CommonRepository._lecturersRepository.GetById(supervisor.LecturerId);
    }
}