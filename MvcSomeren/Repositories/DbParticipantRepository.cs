using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbParticipantRepository : IParticipantRepository
{
    private const string ConnectionStringKey = "appsomeren";
    private readonly string? _connectionString;

    public DbParticipantRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey);
    }

    public List<Participator> GetAll()
    {
        List<Participator> participators = new List<Participator>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId From Participator";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Participator participator;

            while (reader.Read())
            {
                participator = ReadParticipant(reader);
                participators.Add(participator);
            }
            reader.Close();
        }
        return participators;
    }


    public List<Participator> GetAllParticipantsForActivities(int activityId)
    {
        List<Participator> participators = new List<Participator>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId From Participator WHERE ActivityId = @ActivityId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            Participator participator;

            while (reader.Read())
            {
                participator = ReadParticipant(reader);
                FillInParticipant(participator);
                participators.Add(participator);
            }
            reader.Close();
        }
        return participators;
    }

    public Participator? GetById(int id)
    {
        Participator? participator = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId From Participator WHERE ParticipatorId = @ParticipatorId";
            SqlCommand com = new SqlCommand(query, conn);

            com.Parameters.AddWithValue("@ParticipatorId", id);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                participator = ReadParticipant(reader);
                FillInParticipant(participator);
            }
            reader.Close();
        }

        return participator;
    }

    private Participator ReadParticipant(SqlDataReader reader)
    {
        int participatorId = (int)reader["ParticipatorId"];
        int participateDate = (int)reader["ParticipateDate"];
        int studentId = (int)reader["StudentId"];
        int activityId = (int)reader["ActivityId"];


        return new Participator(participatorId, participateDate, studentId, activityId);
    }
    private void FillInParticipant(Participator participator)
    {
        participator.student = CommonRepository._studentRapository.GetById(participator.StudentId);
    }
}