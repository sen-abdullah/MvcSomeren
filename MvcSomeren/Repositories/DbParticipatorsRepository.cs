using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public class DbParticipatorsRepository : IParticipatorsRepository
    {
        private readonly string? _connectionString;

        public DbParticipatorsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("appsomeren");
        }

        public List<Participator> GetAll()
        {
            List<Participator> participators = new List<Participator>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PartcipatorId, ParticipateDate, StudentId, ActivityId FROM Participator ORDER BY PartcipatorId";
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

            return new Participator(participatorId, participateDate, studentId, activityId);
        }

        public void Add(Participator participator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            { /*Correct if wrong*/
                string checkQuery = "SELECT COUNT(*) FROM Participator WHERE ParticipateDate = @ParticipateDate";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@ParticipateDate", participator.ParticipateDate);

                connection.Open();
                int count = (int)checkCommand.ExecuteScalar();
                connection.Close();

                if (count > 0)
                {
                    throw new InvalidOperationException("A participator with this number already exists!");
                }
                //This one is good
                string query = "INSERT INTO Participator (ParticipateDate, StudentId, ActivityId) VALUES (@ParticipateDate, @StudentId, @ActivityId);";
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
        public List<Participator> Filter(int participatorId)
        {
            List<Participator> participators = new List<Participator>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ParticipatorId FROM Participator WHERE ParticipatorId = @ParticipatorId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParticipatorId", participatorId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Participator participatorItem = ReadParticipator(reader);
                    participators.Add(participatorItem);
                }

                reader.Close();
            }

            return participators;
        }
        public Participator? GetById(int participatorId)
        {
            Participator participator = new Participator();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ParticipatorId, ParticipateDate, StudentId, ParticipatorId FROM Participator WHERE ParticipatorId = @ParticipatorId";
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
