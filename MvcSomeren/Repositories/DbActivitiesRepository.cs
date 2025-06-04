using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcSomeren.Models;
using System.Data;

namespace MvcSomeren.Repositories
{
    public class DbActivitiesRepository : IActivitiesRepository
    {
        private readonly string? _connectionString;
        public DbActivitiesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("appsomeren");
        }

        public List<Activity> GetAll()
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
        
        

        private Activity ReadActivity(SqlDataReader reader)
        {
            int activityId = (int)reader["ActivityId"];
            string activityName = (string)reader["ActivityName"];
            string date = (string)reader["Date"];
            string time = (string)reader["Time"];

            return new Activity(activityId, activityName, date, time);
        }

        public void Add(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            { /*Correct if wrong*/
                string checkQuery = "SELECT COUNT(*) FROM Activity WHERE Date = @Date AND Time = @Time";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Date", activity.Date);
                checkCommand.Parameters.AddWithValue("@Time", activity.Time);

                connection.Open();
                int count = (int)checkCommand.ExecuteScalar();
                connection.Close();

                if (count > 0)
                {
                    throw new InvalidOperationException("An activity with this Time already exists!");
                }
                //This one is good
                string query = "INSERT INTO Activity (ActivityName, Date, Time) VALUES (@ActivityName, @Date, @Time);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                command.Parameters.AddWithValue("@Date", activity.Date);
                command.Parameters.AddWithValue("@Time", activity.Time);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public void Update(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Activity SET ActivityName = @ActivityName, Date = @Date, Time = @Time  "
                    + $"WHERE ActivityId = @ActivityId; ";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ActivityId", activity.ActivityId);
                command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                command.Parameters.AddWithValue("@Date", activity.Date);
                command.Parameters.AddWithValue("@Time", activity.Time);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }
        public void Delete(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Activity WHERE ActivityId = @ActivityId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ActivityId", activity.ActivityId);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }
        
        public List<Activity> Filter(string activityName)
        {
            List<Activity> activities = new List<Activity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity WHERE ActivityName LIKE @ActivityName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityName", activityName.Trim() + "%");

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Activity activityItem = ReadActivity(reader);
                    activities.Add(activityItem);
                }

                reader.Close();
            }

            return activities;
        }

        public Activity? GetById(int activityId)
        {
            Activity activity = new Activity();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityId, ActivityName, Date, Time FROM Activity WHERE ActivityId = @ActivityId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityId", activityId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    activity = ReadActivity(reader);
                    FillInSupervisor(activity.Supervisor);
                }

                reader.Close();
            }

            return activity;
        }
        
        private void FillInSupervisor(Supervisor supervisor)
        {
            supervisor.Lecturer = CommonRepository._lecturersRepository.GetById(supervisor.LecturerId);
        }
    }
}
