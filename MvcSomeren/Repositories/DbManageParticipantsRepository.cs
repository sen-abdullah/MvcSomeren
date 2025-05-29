using Microsoft.Data.SqlClient;
using MvcSomeren.Models;
using System.Diagnostics;

namespace MvcSomeren.Repositories
{
    public class DbManageParticipantsRepository : IManageParticipantsRepository
    {
        private const string ConnectionStringKey = "appsomeren";
        private readonly string? _connectionString;

        public DbManageParticipantsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(ConnectionStringKey);
        }


        //It fetches all the orders
        public ManageParticipantViewModel GetAll()
        {
            List<Student> students = new List<Student>();
            List<Models.Activity> activities = new List<Models.Activity>();
            List<Participator> participators = new List<Participator>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                P.ParticipatorId, P.ParticipateDate, P.ActivityId, P.StudentId,
                S.StudentNumber, S.StudentFirstName, S.StudentLastName, S.StudentPhoneNumber, S.StudentClass, S.StudentRoomId,
                A.ActivityId, A.ActivityName, A.Date, A.Time
            FROM Participator AS P
            JOIN Student AS S ON P.StudentId = S.StudentId
            JOIN Activity AS A ON P.ActivityId = A.ActivityId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);

                    Models.Activity activity = ReadActivity(reader);
                    activities.Add(activity);

                    int participateDate = (int)reader["ParticipateDate"];

                    int participatorId = (int)reader["ParticipatorId"];
                    int studentId = (int)reader["StudentId"];
                    int activityId = (int)reader["ActivityId"];

                    Participator participator = new Participator(participatorId,  studentId, activityId, participateDate);
                    participators.Add(participator);

                }

                reader.Close();
            }

            return new ManageParticipantViewModel(students, activities, null, GetStudents(), GetActivities(), participators);
        }

        public ManageParticipantViewModel GetStudentsAndActivities()
        {
            return new ManageParticipantViewModel(new List<Student>(), new List<Models.Activity>(), null, GetStudents(), GetActivities(), new List<Participator>());
        }

        public void AddParticipator(ManageParticipantViewModel manageParticipantViewModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    $"INSERT INTO Participator (ActivityId, StudentId, ParticipateDate)" +
                    $"VALUES (@ActivityId, @StudentId, @ParticipateDate); " +
                    "SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                if (manageParticipantViewModel.Participator != null)
                {
                    command.Parameters.AddWithValue("@ActivityId", manageParticipantViewModel.Participator.ActivityId);
                    command.Parameters.AddWithValue("@StudentId", manageParticipantViewModel.Participator.StudentId);
                    command.Parameters.AddWithValue("@ParticipateDate", manageParticipantViewModel.Participator.ParticipateDate);
                }

                command.Connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected != 1) throw new Exception("Adding a new participator failed.");
            }
        }
        public void UpdateParticipator(ManageParticipantViewModel manageParticipantViewModel)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    $"UPDATE Participator SET ActivityId = @ActivityId, StudentId = @StudentId, ParticipateDate = @ParticipateDate WHERE ParticipatorId = @ParticipatorId; ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityId", manageParticipantViewModel.Participator.ActivityId);
                command.Parameters.AddWithValue("@StudentId", manageParticipantViewModel.Participator.StudentId);
                command.Parameters.AddWithValue("@ParticipateDate", manageParticipantViewModel.Participator.ParticipateDate);
                command.Parameters.AddWithValue("@ParticipatorId", manageParticipantViewModel.Participator.ParticipatorId);
                command.Connection.Open();
                var numberOfRowsAffected = command.ExecuteNonQuery();
                if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Participant was not updated.");
            }
        }
        public void Delete(int participatorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = $"DELETE FROM Participator WHERE ParticipatorId = @ParticipatorId;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParticipatorId", participatorId);

                try
                {
                    connection.Open();
                    var numberOfRowsAffected = command.ExecuteNonQuery();

                    if (numberOfRowsAffected != 1)
                    {
                        throw new Exception($"Delete failed. Rows affected: {numberOfRowsAffected}. ParticipatorId: {participatorId}.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting Participator: " + ex.Message, ex);
                }
            }
        }

        public void Add(Participator participator)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Participator (ParticipateDate, StudentId, ActivityId) VALUES (@ParticipateDate, @StudentId, @ActivityId)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ParticipateDate", participator.ParticipateDate);
                command.Parameters.AddWithValue("@StudentId", participator.StudentId);
                command.Parameters.AddWithValue("@ActivityId", participator.ActivityId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }








        public ManageParticipantViewModel GetParticipatorByID(int participatorId)
        {
            List<Student> students = new List<Student>();
            List<Models.Activity> activities = new List<Models.Activity>();

            Participator? participator = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                P.ParticipatorId, P.ParticipateDate, P.ActivityId, P.StudentId,
                S.StudentNumber, S.StudentFirstName, S.StudentLastName, S.StudentPhoneNumber, S.StudentClass, S.StudentRoomId,
                A.ActivityId, A.ActivityName, A.Date, A.Time
            FROM Participator AS P
            JOIN Student AS S ON P.StudentId = S.StudentId
            JOIN Activity AS A ON P.ActivityId = A.ActivityId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParticipatorId", participatorId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);

                    Models.Activity activity = ReadActivity(reader);
                    activities.Add(activity);

                    int participateDate = (int)reader["ParticipateDate"];

                    int id = (int)reader["ParticipatorId"];
                    int studentId = (int)reader["StudentId"];
                    int activityId = (int)reader["ActivityId"];

                    participator = new Participator(id, studentId, activityId, participateDate);
                }

                reader.Close();
            }

            return new ManageParticipantViewModel(students, activities, participator, GetStudents(), GetActivities(), new List<Participator>());
        }

        public Student? GetStudentById(int id)
        {
            Student? student = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query =
                    "SELECT StudentId, StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, StudentRoomId FROM Student WHERE StudentId = @StudentId;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", id);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    student = ReadStudent(reader);

                }

                reader.Close();
            }
            return student;

        }

        public Models.Activity? GetActivityById(int id)
        {
            Models.Activity? activity = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity WHERE ActivityId = @ActivityId;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityId", id); 

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    activity = ReadActivity(reader);
                }

                reader.Close();
            }

            return activity;
        }
        private List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT StudentId, StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, StudentRoomId FROM Student ORDER BY StudentLastName ASC";

                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);
                }

                reader.Close();
            }

            return students;
        }
        private List<Models.Activity> GetActivities()
        {
            List<Models.Activity> activities = new List<Models.Activity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {        
                string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity ORDER BY ActivityId";

                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Models.Activity activity = ReadActivity(reader);
                    activities.Add(activity);
                }

                reader.Close();
            }

            return activities;
        }
        private Student ReadStudent(SqlDataReader reader)
        {
            int studentId = (int)reader["StudentId"];
            int studentNumber = (int)reader["StudentNumber"];
            string studentFirstName = (string)reader["StudentFirstName"];
            string studentLastName = (string)reader["StudentLastName"];
            int studentPhoneNumber = (int)reader["StudentPhoneNumber"];
            string studentClass = (string)reader["StudentClass"];
            int studentRoomId = (int)reader["StudentRoomId"];


            return new Student(
                studentId,
                studentNumber,
                studentFirstName,
                studentLastName,
                studentPhoneNumber,
                studentClass,
                studentRoomId
            );
        }

        private Models.Activity ReadActivity(SqlDataReader reader)
        {
            int activityId = (int)reader["ActivityId"];
            string activityName = (string)reader["ActivityName"];
            string date = (string)reader["Date"];
            string time = (string)reader["Time"];

            return new Models.Activity(activityId, activityName, date, time);
        }

        private Participator ReadParticipator(SqlDataReader reader)
        {
            int participatorId = (int)reader["ParticipatorId"];
            int participateDate = (int)reader["ParticipateDate"];
            int studentId = (int)reader["StudentId"];
            int activityId = (int)reader["ActivityId"];


            return new Participator(participatorId, participateDate, studentId, activityId);
        }
    }
}
