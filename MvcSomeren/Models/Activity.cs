namespace MvcSomeren.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }

        public Activity()
        {
            ActivityId = 0;
            ActivityName = "";
        }

        public Activity(int activityId, string activityName)
        {
            ActivityId = activityId;
            ActivityName = activityName;
        }
    }
}
