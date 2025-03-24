using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public class DbStudentRepository : IStudentRapository
{
    private readonly string? _connectionString;
    private const string CONNECTION_STRING_KEY = "appsomeren";

    public DbStudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(CONNECTION_STRING_KEY);
    }
    // Checked!!
    public List<Student> GetAll()
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
              "SELECT StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, Voucher, RoomId FROM Student ORDER BY StudentLastName ASC";
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
    
    // Checked!!
    public Student? GetById(int id)
    {
        Student? student = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                $"SELECT StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, Voucher, RoomId FROM Student WHERE StudentNumber = @StudentNumber";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentNumber", id);

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

    // Checked!!
    public void AddStudent(Student student)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"INSERT INTO Student (StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, Voucher, RoomId )" +
                $"VALUES (@StudentNumber, @StudentFirstName, @StudentLastName, @StudentPhoneNumber, @StudentClass, @Voucher, @RoomId); ";
                //"SELECT CAST(SCOPE_IDENTITY() as int)"

            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);
            command.Parameters.AddWithValue("@StudentFirstName", student.StudentFirstName);
            command.Parameters.AddWithValue("@StudentLastName", student.StudentLastName);
            command.Parameters.AddWithValue("@StudentPhoneNumber", student.StudentPhoneNumber);
            command.Parameters.AddWithValue("@StudentClass", student.StudentClass);
            command.Parameters.AddWithValue("@Voucher", student.Voucher);
            command.Parameters.AddWithValue("@RoomId", student.RoomId);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Adding a new Student failed.");
        }
    }

    // Checked!!
    public void UpdateStudent(Student student)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query =
                $"UPDATE Student SET StudentFirstName = @StudentFirstName, StudentLastName = @StudentLastName, StudentPhoneNumber = @StudentPhoneNumber, StudentClass = @StudentClass, Voucher = @Voucher, RoomId = @RoomId " +
                $"WHERE StudentNumber = @StudentNumber; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentFirstName", student.StudentFirstName);
            command.Parameters.AddWithValue("@StudentLastName", student.StudentLastName);
            command.Parameters.AddWithValue("@StudentPhoneNumber", student.StudentPhoneNumber);
            command.Parameters.AddWithValue("@StudentClass", student.StudentClass);
            command.Parameters.AddWithValue("@Voucher", student.Voucher);
            command.Parameters.AddWithValue("@RoomId", student.RoomId);
            command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Student was not updated.");
        }
    }
    
    //Checked!!
    public void DeleteStudent(Student student)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            var query = $"DELETE FROM Student WHERE StudentNumber = @StudentNumber;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);

            command.Connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            if (numberOfRowsAffected != 1) throw new Exception("Something went wrong! Student was not deleted.");
        }
    }

    //Checked!!
    public bool IsStudentExist(Student student)
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, Voucher, RoomId FROM Student WHERE StudentNumber = @StudentNumber;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentNumber", student.StudentNumber);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student studentItem= ReadStudent(reader);
                students.Add(studentItem);
            }

            reader.Close();
        }

        return students.Count > 0;
    }


    //Checked!!
    public List<Student> Filter(string studentLastName)
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT StudentNumber, StudentFirstName, StudentLastName, StudentPhoneNumber, StudentClass, Voucher, RoomId FROM Student WHERE StudentLastName = @StudentLastName;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentLastName", studentLastName.Trim());

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Student studentItem = ReadStudent(reader);
                students.Add(studentItem);
            }

            reader.Close();
        }

        return students;
    }

    //Checked!!
    private Student ReadStudent(SqlDataReader reader)
    {
        int studentNumber = (int)reader["StudentNumber"];
        string studentFirstName = (string)reader["StudentFirstName"];
        string studentLastName = (string)reader["StudentLastName"];
        int studentPhoneNumber = (int)reader["StudentPhoneNumber"];
        string studentClass = (string)reader["StudentClass"];
        bool voucher = (bool)reader["Voucher"];
        int roomId = (int)reader["RoomId"];
        

        return new Student(studentNumber, studentFirstName, studentLastName, studentPhoneNumber, studentClass, voucher, roomId);
    }
}