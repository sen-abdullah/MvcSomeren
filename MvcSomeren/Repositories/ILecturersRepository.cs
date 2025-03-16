using System.Collections.Generic;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface ILecturersRepository
{
    List<Lecturer> GetAll();
}