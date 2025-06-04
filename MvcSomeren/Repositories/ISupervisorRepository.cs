using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ISupervisorRepository
{
    List<Supervisor> GetAll();
    List<Supervisor> GetAllSupervisorsForActivities(int activityId);
    List<Supervisor> GetAllSupervisorsWithoutActivities(int activityId);
    void AddSupervisor(Supervisor supervisor, int activityId);
    Supervisor? GetById(int id);
}