using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IParticipantRepository
{
    List<Participator> GetAll();
    List<Participator> GetAllParticipantsForActivities(int activityId);
    List<Student> GetAllStudentsWithoutActivity(int activityId);
    Participator? GetById(int id);

    void AddParticipant(Participator participator, int activityId);

    void AddStudent(int studentId, int activityId);
}