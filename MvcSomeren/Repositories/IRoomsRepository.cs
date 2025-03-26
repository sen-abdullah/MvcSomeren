using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IRoomsRepository
{
    List<Room> GetAll();
    void Add(Room room);
    void Update(Room room);
    void Delete(Room room);

    List<Room> Filter(int roomSize);
    Room? GetById(int roomId);
}
