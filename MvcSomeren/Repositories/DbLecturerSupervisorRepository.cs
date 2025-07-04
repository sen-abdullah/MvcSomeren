using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbLecturerSupervisorRepository : ILecturerSupervisorRepository
{
    private const string ConnectionStringKey = "appsomeren";
    private readonly string? _connectionString;

    public DbLecturerSupervisorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey);
    }

    
    public LecturerSupervisorViewModel GetAll()
    {
        List<Lecturer> lecturers = new List<Lecturer>();
        List<Activity> activities = new List<Activity>();
        List<Supervisor> supervisors = new List<Supervisor>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT Supervisor.SupervisorId, Supervisor.SupervisingDate, Activity.ActivityId, Activity.ActivityName, Activity.Date, Activity.Time, Lecturer.LecturerId, Lecturer.LecturerFirstName, Lecturer.LecturerLastName, Lecturer.LecturerPhoneNumber, Lecturer.LecturerAge, Lecturer.RoomId FROM Supervisor JOIN Activity ON Supervisor.ActivityId = Activity.ActivityId JOIN Lecturer ON Supervisor.LecturerId = Lecturer.LecturerId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Activity activity = ReadActivity(reader);
                activities.Add(activity);
                
                Lecturer lecturer = ReadLecturer(reader);
                lecturers.Add(lecturer);
                
                int supervisingDate = (int)reader["SupervisingDate"];
                
                int supervisorId = (int)reader["SupervisorId"];
                int activityId = (int)reader["ActivityId"];
                int lecturerId = (int)reader["LecturerId"];
                
                Supervisor supervisor = new Supervisor(supervisorId, supervisingDate, lecturerId, activityId);
                supervisors.Add(supervisor);
                
            }

            reader.Close();
        }

        return new LecturerSupervisorViewModel(lecturers, activities, null, GetLecturers(), GetActivities(), supervisors);
    }

    public LecturerSupervisorViewModel GetLecturersAndActivities()
    {
        return new LecturerSupervisorViewModel(new List<Lecturer>(), new List<Activity>(), null, GetLecturers(), GetActivities(), new List<Supervisor>());
    }
    /*

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
    */
    
    /*
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
    */
    
    public void UpdateSupervisor(LecturerSupervisorViewModel lecturerSupervisorViewModel)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = @"UPDATE Supervisor SET SupervisingDate = @SupervisingDate, LecturerId = @LecturerId, ActivityId = @ActivityId WHERE SupervisorId = @Id; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupervisingDate", lecturerSupervisorViewModel.Supervisor.SupervisingDate);
            command.Parameters.AddWithValue("@LecturerId", lecturerSupervisorViewModel.Supervisor.LecturerId);
            command.Parameters.AddWithValue("@ActivityId", lecturerSupervisorViewModel.Supervisor.ActivityId);
            command.Parameters.AddWithValue("@Id", lecturerSupervisorViewModel.Supervisor.SupervisorId);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Supervisor was not updated.");
        }

    }

    public LecturerSupervisorViewModel GetSupervisorById(int supervisorId)
    {
        List<Lecturer> lecturers = new List<Lecturer>();
        List<Activity> activities  = new List<Activity>();
        Supervisor? supervisor = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT Supervisor.SupervisorId, Supervisor.SupervisingDate, Activity.ActivityId, Activity.ActivityName, Activity.Date, Activity.Time, Lecturer.LecturerId, Lecturer.LecturerFirstName, Lecturer.LecturerLastName, Lecturer.LecturerPhoneNumber, Lecturer.LecturerAge, Lecturer.RoomId FROM Supervisor JOIN Activity ON Supervisor.ActivityId = Activity.ActivityId JOIN Lecturer ON Supervisor.LecturerId = Lecturer.LecturerId WHERE Supervisor.SupervisorId = @SupervisorId;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SupervisorId", supervisorId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Activity activity = ReadActivity(reader);
                activities.Add(activity);
                
                Lecturer lecturer = ReadLecturer(reader);
                lecturers.Add(lecturer);
                
                int supervisingDate = (int)reader["SupervisingDate"];
                
                int id = (int)reader["SupervisorId"];
                int activityId = (int)reader["ActivityId"];
                int lecturerId = (int)reader["LecturerId"];
                
                supervisor = new Supervisor(id, supervisingDate, lecturerId, activityId);
            }

            reader.Close();
        }

        return new LecturerSupervisorViewModel(lecturers, activities, supervisor, GetLecturers(), GetActivities(), new List<Supervisor>());
    }

    public Activity GetActivityById(int id)
    {
        Activity? activity = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity WHERE ActivityId = @ActivityId";

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

    public Lecturer? GetLecturerById(int id)
    {
        Lecturer? lecturer = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId FROM Lecturer WHERE LecturerId = @LecturerId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LecturerId", id);

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


    private List<Activity> GetActivities()
    {
        List<Activity> activities = new List<Activity>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity ORDER BY ActivityId";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Activity activity = ReadActivity(reader);
                activities.Add(activity);
            }

            reader.Close();
        }

        return activities;
    }

    private List<Lecturer> GetLecturers()
    {
        List<Lecturer> lecturers = new List<Lecturer>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId FROM Lecturer ORDER BY LecturerLastName ASC";
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
    


    private Activity ReadActivity(SqlDataReader reader)
    {
        int activityId = (int)reader["ActivityId"];
        string activityName = (string)reader["ActivityName"];
        string date = (string)reader["Date"];
        string time = (string)reader["Time"];


        return new Activity(
            activityId,
            activityName,
            date,
            time
        );
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
    
    private Supervisor ReadSupervisor(SqlDataReader reader)
    {
        int supervisorId = (int)reader["SupervisorId"];
        int supervisingDate = (int)reader["SupervisingDate"];
        int lecturerId = (int)reader["LecturerId"];
        int activityId = (int)reader["ActivityId"];
        

        return new Supervisor(supervisorId, supervisingDate, lecturerId, activityId);
    }

}