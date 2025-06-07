using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;
public interface IParticipantsRepository
{
    List<Participator> GetAll();
    List<Participator> GetAllParticipantsForActivities(int activityId);
    public List<Student> GetAllStudentsWithoutActivity(int activityId);
    void DeleteParticipant(int participantId, int activityId);
    void AddParticipant(Participator participator, int activityId);
    void AddStudent(int studentId, int activityId);
    Participator? GetById(int id);
}