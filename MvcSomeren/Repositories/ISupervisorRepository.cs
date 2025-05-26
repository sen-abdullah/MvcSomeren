using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ISupervisorRepository
{
    List<Supervisor> GetAll();
    List<Supervisor> GetAllSupervisorsForActivities(int activityId);
    Supervisor? GetById(int id);
}