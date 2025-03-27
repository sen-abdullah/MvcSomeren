using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public class DbRoomsRepository : IRoomsRepository
    {
        private readonly string? _connectionString;

        public DbRoomsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("appsomeren");
        }

        public List<Room> GetAll()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, RoomSize, RoomType, Building, Floor FROM Room ORDER BY RoomNumber";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Room room = ReadRoom(reader);
                    rooms.Add(room);
                }

                reader.Close();
            }

            return rooms;
        }

        private Room ReadRoom(SqlDataReader reader)
        {
            int roomId = (int)reader["RoomId"];
            int roomNumber = (int)reader["RoomNumber"];
            int roomSize = (int)reader["RoomSize"];
            string roomType = (string)reader["RoomType"];
            string building = (string)reader["Building"];
            string floor = (string)reader["Floor"];

            return new Room(roomId, roomNumber, roomSize, roomType, building, floor);
        }

        public void Add(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            { /*Correct if wrong*/
                string checkQuery = "SELECT COUNT(*) FROM Room WHERE RoomNumber = @RoomNumber";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);

                connection.Open();
                int count = (int)checkCommand.ExecuteScalar();
                connection.Close();

                if (count > 0)
                {
                    throw new InvalidOperationException("A room with this number already exists!");
                }
                //This one is good
                string query = "INSERT INTO Room (RoomNumber, RoomSize, RoomType, Building, Floor) VALUES (@RoomNumber, @RoomSize, @RoomType, @Building, @Floor);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                command.Parameters.AddWithValue("@RoomType", room.RoomType);
                command.Parameters.AddWithValue("@Building", room.Building);
                command.Parameters.AddWithValue("@Floor", room.Floor);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public void Update(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Room SET RoomNumber = @RoomNumber, RoomSize = @RoomSize, RoomType = @RoomType, Building = @Building, Floor = @Floor  "
                    + $"WHERE RoomId = @RoomId; ";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@RoomId", room.RoomId);
                command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                command.Parameters.AddWithValue("@RoomSize", room.RoomSize);
                command.Parameters.AddWithValue("@RoomType", room.RoomType);
                command.Parameters.AddWithValue("@Building", room.Building);
                command.Parameters.AddWithValue("@Floor", room.Floor);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public void Delete(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Room WHERE RoomId = @RoomId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@RoomId", room.RoomId);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public List<Room> Filter(int roomSize)
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, RoomSize, RoomType, Building, Floor FROM Room WHERE RoomSize = @RoomSize";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomSize", roomSize);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Room roomItem = ReadRoom(reader);
                    rooms.Add(roomItem);
                }

                reader.Close();
            }

            return rooms;
        }
        public Room? GetById(int roomId)
        {
            Room room = new Room();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoomId, RoomNumber, RoomSize, RoomType, Building, Floor FROM Room WHERE RoomId = @RoomId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomId", roomId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    room = ReadRoom(reader);
                }

                reader.Close();
            }

            return room;
        }
    }
}
