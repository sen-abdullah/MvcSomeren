using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ILecturersRepository
{
    List<Lecturer> GetAll();

    Lecturer? GetById(int id);

    void Add(Lecturer lecturer);

    void Update(Lecturer lecturer);

    void Delete(Lecturer lecturer);

    bool IsLecturerExist(Lecturer lecturer);
    
    List<Lecturer> Filter(String lastName);
}