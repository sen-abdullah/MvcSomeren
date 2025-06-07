using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public interface IManageParticipantsRepository
    {
        ManageParticipantViewModel GetAll();

        void DeleteParticipant(int participantId, int activityId);
        ManageParticipantViewModel GetStudentsAndActivities();
        void AddParticipator(ManageParticipantViewModel manageParticipantViewModel);

        void Delete(int participatorId);

        ManageParticipantViewModel GetParticipatorByID(int participatorId);

        Student? GetStudentById(int id);

        Activity? GetActivityById(int id);

        void UpdateParticipator(ManageParticipantViewModel manageParticipantViewModel);
    }
}

