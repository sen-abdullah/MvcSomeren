using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IStudentRapository
{
    List<Student> GetAll();
    List<Student> GetStudentsNotInActivity(int activityId);
    Student? GetById(int id);

    void AddStudent(Student student);

    void UpdateStudent(Student student);

    void DeleteStudent(Student student);

    bool IsStudentExist(Student student);
    
    List<Student> Filter(string lastName);
    
    bool IsRoomIdExist(Student student);
}