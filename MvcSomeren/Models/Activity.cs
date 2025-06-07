namespace MvcSomeren.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        
        public Supervisor Supervisor { get; set; }

        public Activity()
        {
            ActivityId = 0;
            ActivityName = "";
            Date = "";
            Time = "";
        }

        public Activity(int activityId, string activityName, string date, string time)
        {
            ActivityId = activityId;
            ActivityName = activityName;
            Date = date;
            Time = time;
        }
    }
}
