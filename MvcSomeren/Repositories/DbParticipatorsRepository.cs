using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public class DbParticipatorsRepository : IParticipatorsRepository
    {
        private readonly string? _connectionString;
        private const string CONNECTION_STRING_KEY = "appsomeren";

        public DbParticipatorsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
        }

        public List<Participator> GetAll()
        {
            List<Participator> participators = new List<Participator>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //string query = "SELECT P.PartcipatorId, P.ParticipateDate, S.StudentId, P.ActivityId, S.StudentNumber, S.StudentFirstName, S.StudentLastName, S.StudentPhoneNumber, S.StudentClass, S.StudentRoomId FROM Participator AS P JOIN Student AS S ON P.StudentId = S.StudentId ORDER BY PartcipatorId";
                string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId FROM Participator";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Participator participator = ReadParticipator(reader);
                    participators.Add(participator);
                }

                reader.Close();
            }

            return participators;
        }

        private Participator ReadParticipator(SqlDataReader reader)
        {
            int participatorId = (int)reader["ParticipatorId"];
            int participateDate = (int)reader["ParticipateDate"];
            int studentId = (int)reader["StudentId"];
            int activityId = (int)reader["ActivityId"];

            //int studentNumber = (int)reader["StudentNumber"];
            //string studentFirstName = (string)reader["StudentFirstName"];
            //string studentLastName = (string)reader["StudentLastName"];
            //int studentPhoneNumber = (int)reader["StudentPhoneNumber"];
            //string studentClass = (string)reader["StudentClass"];
            //int roomId = (int)reader["RoomId"];

            return new Participator(participatorId, participateDate, studentId, activityId);
            //, studentNumber, studentFirstName, studentLastName, studentPhoneNumber, studentClass, roomId
        }

        public void Add(Participator participator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            { 
                string query = "INSERT INTO Participator (ParticipateDate, StudentId, ActivityId) VALUES (@ParticipateDate, @StudentId, @ActivityId); SELECT CAST(SCOPE_IDENTITY() as int)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ParticipateDate", participator.ParticipateDate);
                command.Parameters.AddWithValue("@StudentId", participator.StudentId);
                command.Parameters.AddWithValue("@ActivityId", participator.ActivityId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public void Update(Participator participator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Participator SET ParticipateDate = @ParticipateDate, StudentId = @StudentId, ActivityId = @ActivityId  "
                    + $"WHERE ParticipatorId = @ParticipatorId; ";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ParticipateDate", participator.ParticipateDate);
                command.Parameters.AddWithValue("@StudentId", participator.StudentId);
                command.Parameters.AddWithValue("@ActivityId", participator.ActivityId);
                command.Parameters.AddWithValue("@ParticipatorId", participator.ParticipatorId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public void Delete(Participator participator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Partcipator WHERE ParticipatorId = @ParticipatorId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ParticipatorId", participator.ParticipatorId);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public Participator? GetById(int participatorId)
        {
            Participator? participator = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ActivityId FROM Participator WHERE ParticipatorId = @ParticipatorId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParticipatorId", participatorId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    participator = ReadParticipator(reader);
                }

                reader.Close();
            }

            return participator;
        }
    }
}
