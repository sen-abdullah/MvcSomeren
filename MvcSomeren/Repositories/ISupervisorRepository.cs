using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ISupervisorRepository
{
    List<Supervisor> GetAll();
    List<Supervisor> GetAllSupervisorsForActivities(int activityId);
    List<Lecturer> GetAllLecturersNotSupervisingActivity(int activityId);
    void DeleteSupervisor(int supervisorId, int activityId);
    void AddSupervisor(Supervisor supervisor, int activityId);
    Supervisor? GetById(int id);
}