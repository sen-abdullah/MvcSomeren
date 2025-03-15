using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IStudentRapository
{
    List<Student> GetAll();
}