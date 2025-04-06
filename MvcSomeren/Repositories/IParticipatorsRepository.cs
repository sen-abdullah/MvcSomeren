using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IParticipatorsRepository
{
    List<Participator> GetAll();
    void Add(Participator participator);
    void Update(Participator participator);
    void Delete(Participator participator);
    Participator? GetById(int participatorId);
}

