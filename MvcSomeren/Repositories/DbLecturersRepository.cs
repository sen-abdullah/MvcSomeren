using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbLecturersRepository : ILecturersRepository
{
    private readonly string? _connectionString;
    private const string CONNECTION_STRING_KEY = "appsomeren";

    public DbLecturersRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
    }

    public List<Lecturer> GetAll()
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

    public Lecturer? GetById(int id)
    {
        Lecturer? lecturer = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId FROM Lecturer WHERE LecturerId = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

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


    public void Add(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO Lecturer (LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId)" +
                $"VALUES (@FirstName, @LastName, @PhoneNumber, @Age, @RoomId); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
            command.Parameters.AddWithValue("@LastName", lecturer.LastName);
            command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
            command.Parameters.AddWithValue("@Age", lecturer.Age);
            command.Parameters.AddWithValue("@RoomId", lecturer.RoomId);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Adding a new Lecturer failed.");
        }
    }

    public void Update(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE Lecturer SET LecturerFirstName = @FirstName, LecturerLastName = @LastName, LecturerPhoneNumber = @PhoneNumber, LecturerAge = @Age, RoomId = @RoomId " +
                $"WHERE LecturerId = @Id; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
            command.Parameters.AddWithValue("@LastName", lecturer.LastName);
            command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
            command.Parameters.AddWithValue("@Age", lecturer.Age);
            command.Parameters.AddWithValue("@RoomId", lecturer.RoomId);
            command.Parameters.AddWithValue("@Id", lecturer.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not updated.");
        }
    }

    public void Delete(Lecturer lecturer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Lecturer WHERE LecturerId = @Id;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", lecturer.Id);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Lecturer was not deleted.");
        }
    }

    public bool IsLecturerExist(Lecturer lecturer)
    {
        List<Lecturer> lecturers = new List<Lecturer>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT LecturerId, LecturerFirstName, LecturerLastName, LecturerPhoneNumber, LecturerAge, RoomId FROM Lecturer WHERE LecturerFirstName = @FirstName AND LecturerLastName = @LastName;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", lecturer.FirstName.Trim());
            command.Parameters.AddWithValue("@LastName", lecturer.LastName.Trim());

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Lecturer lecturerItem = ReadLecturer(reader);
                lecturers.Add(lecturerItem);
            }

            reader.Close();
        }

        return lecturers.Count > 0;
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
}