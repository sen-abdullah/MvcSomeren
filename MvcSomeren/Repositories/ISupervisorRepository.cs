using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ISupervisorRepository
{
    List<Supervisor> GetAll();

    Supervisor? GetById(int id);

    void Add(Supervisor supervisor);

    void Update(Supervisor supervisor);

    void Delete(Supervisor supervisor);

    bool IsSupervisorExist(Supervisor supervisor);
    
    List<Supervisor> Filter(String lastName);
}