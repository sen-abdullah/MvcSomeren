namespace MvcSomeren.Models;

public class Supervisor : Lecturer
{
    public int SupervisorId { get; set; }
    public int SupervisingDate { get; set; }
    public int LecturerId { get; set; }
    public int ActivityId { get; set; }

    public Supervisor()
    {
        SupervisorId = 0;
        SupervisingDate = 0;
        LecturerId = 0;
        ActivityId = 0;
    }

    public Supervisor(int supervisorId, int supervisingDate, int lecturerId, int activityId, string firstName, string lastName, string phoneNumber, int age, int roomId) : base(lecturerId,firstName, lastName, phoneNumber, age, roomId)
    {
        SupervisorId = supervisorId;
        SupervisingDate = supervisingDate;
        LecturerId = lecturerId;
        ActivityId = activityId;
    }
}