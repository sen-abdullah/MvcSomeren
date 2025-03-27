using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ISupervisorRepository
{
    List<Supervisor> GetAll();

    Supervisor? GetById(int id);

    void Add(Supervior supervisor);

    void Update(Supervisor supervisor);

    void Delete(Supervisor supervisor);

    bool IsLecturerExist(Supervisor supervisor);
    
    List<Supervisor> Filter(String lastName);
    
    bool IsRoomExist(Supervisor supervisor);
}