using Microsoft.Data.SqlClient;
using MvcSomeren.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MvcSomeren.Repositories;

public class DbParticipantRepository : IParticipantsRepository
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
            string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId From Participator WHERE ActivityId = @ActivityId ORDER BY ParticipateDate ASC";
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

    public List<Student> GetAllStudentsWithoutActivity(int activityId)
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT StudentId, StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, StudentRoomId FROM Student 
                         WHERE StudentId NOT IN (
                             SELECT StudentId FROM Participator WHERE ActivityId = @ActivityId
                         ) ORDER BY StudentLastName ASC";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student student = ReadStudent(reader);
                students.Add(student);
            }
        }

        return students;
    }
    public void AddParticipant(Participator participator, int activityId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    $"INSERT INTO Participator (ParticipatorId, ParticipateDate, StudentId, ActivityId)" +
                    $"VALUES (@ParticipatorId, @ParticipateDate, @StudentId, @ActivityId); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                SqlCommand command = new SqlCommand(query, connection);

                if (activityId == participator.ActivityId)
                {
                    command.Parameters.AddWithValue("@ParticipatorId", participator.ParticipatorId);
                    command.Parameters.AddWithValue("@ParticipateDate", participator.ParticipateDate);
                    command.Parameters.AddWithValue("@StudentId", participator.StudentId);
                    command.Parameters.AddWithValue("@ActivityId", participator.ActivityId);
                }

                command.Connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected != 1) throw new Exception("Adding a new Participant failed.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void DeleteParticipant(int participantId, int activityId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Participator WHERE ParticipatorId = @ParticipatorId AND ActivityId = @ActivityId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ParticipatorId", participantId);
            command.Parameters.AddWithValue("@ActivityId", activityId);

            try
            {
                connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();

                if (numberOfRowsAffected != 1)
                {
                    throw new Exception($"Delete failed. Rows affected: {numberOfRowsAffected}. ParticipatorId: {participantId}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Supervisor: " + ex.Message, ex);
            }
        }
    }

    public void AddStudent(int studentId, int activityId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"
                INSERT INTO Participator (ParticipateDate, StudentId, ActivityId)
                VALUES (@ParticipateDate, @StudentId, @ActivityId);
            "
                ; 

                SqlCommand command = new SqlCommand(query, connection);

                int supervisingDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                command.Parameters.AddWithValue("@ParticipateDate", supervisingDate);
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@ActivityId", activityId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    throw new Exception("Adding a new Participant failed. No rows affected.");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
        return new Participator((int)reader["ParticipatorId"], (int)reader["ParticipateDate"], (int)reader["StudentId"], (int)reader["ActivityId"]);
    }
    private Student ReadStudent(SqlDataReader reader)
    {
        int studentId = (int)reader["StudentId"];
        int studentNumber = (int)reader["StudentNumber"];
        string studentFirstName = (string)reader["StudentFirstName"];
        string studentLastName = (string)reader["StudentLastName"];
        int studentPhoneNumber = (int)reader["StudentPhoneNumber"];
        string studentClass = (string)reader["StudentClass"];
        int? roomId = (int)reader["StudentRoomId"];

        return new Student(studentId, studentNumber, studentFirstName, studentLastName, studentPhoneNumber, studentClass, roomId);
    }
    private void FillInParticipant(Participator participator)
    {

        if (participator == null)
            throw new ArgumentNullException(nameof(participator), "Participator is null");

        if (CommonRepository._studentRapository == null)
            throw new InvalidOperationException("Participators repository is not initialized");

        var student = CommonRepository._studentRapository.GetById(participator.StudentId);
        if (student == null)
            throw new Exception($"No student found with ID {participator.StudentId}");

        participator.Student = student;

        participator.Student = CommonRepository._studentRapository.GetById(participator.StudentId);
    }
}