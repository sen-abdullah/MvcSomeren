namespace MvcSomeren.Models;

public class ManageActivityViewModel
{
    public int ActivityID { get; set; }
    public Activity Activity { get; set; }
    public List<Supervisor> Supervisors { get; set; }
    public List<Lecturer> Lecturers { get; set; }
    public List<Participator> Participators { get; set; }
    public List<Student> Students { get; set; }
}