using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public interface IManageParticipantsRepository
    {
        ManageParticipantViewModel GetAll();
        ManageParticipantViewModel GetStudentsAndActivities();
        void AddParticipator(ManageParticipantViewModel manageParticipantViewModel);

        void Delete(int participatorId);

        ManageParticipantViewModel GetParticipatorByID(int participatorId);

        Student? GetStudentById(int id);

        Activity? GetActivityById(int id);
    }
}
