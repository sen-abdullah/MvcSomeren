namespace MvcSomeren.Models;

public class ManageActivityViewModel
{
    public int ActivityID { get; set; }
    public List<Supervisor> Supervisors { get; set; }
    public List<Participator> Participators { get; set; }
}