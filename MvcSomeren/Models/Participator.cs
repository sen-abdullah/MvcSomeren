namespace MvcSomeren.Models
{
    public class Participator 
    {
        public int ParticipatorId { get; set; }
        public int ParticipateDate { get; set; }
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public Participator()
        {
            ParticipatorId = 0;
            ParticipateDate = 0;
            StudentId = 0;
            ActivityId = 0;
        }

        public Participator(int partcipatorId, int participateDate, int studentId, int activityId)
        {
            ParticipatorId = partcipatorId;
            ParticipateDate = participateDate;
            StudentId = studentId;
            ActivityId = activityId;
        }
    }
}
