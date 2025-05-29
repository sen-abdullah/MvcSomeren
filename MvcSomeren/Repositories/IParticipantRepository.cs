using Microsoft.AspNetCore.Mvc;
using MvcSomeren.Models;

namespace MvcSomeren.Repositories
{
    public interface IParticipantRepository
    {
        List<Participator> GetAll();
        List<Participator> GetAllParticipantsForActivities(int activityId);
        Participator? GetById(int id);
    }
}
