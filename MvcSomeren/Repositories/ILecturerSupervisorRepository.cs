using MvcSomeren.Models;
using Activity = System.Diagnostics.Activity;

namespace MvcSomeren.Repositories;

public interface ILecturerSupervisorRepository
{
    LecturerSupervisorViewModel GetAll();
    LecturerSupervisorViewModel GetLecturersAndActivities();
    void UpdateSupervisor(LecturerSupervisorViewModel lecturerSupervisorViewModel);

    LecturerSupervisorViewModel GetSupervisorById(int supervisorId);

    Models.Activity GetActivityById(int id);

    Lecturer? GetLecturerById(int id);
}