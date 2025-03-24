using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IRoomsRepository
{
    List<Room> GetAll();
    void Add(Room room);
    void Update(Room room);
    void Delete(Room room);

    Room? GetById(int roomId);
}
