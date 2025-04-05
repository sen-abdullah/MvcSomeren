using Microsoft.AspNetCore.Authorization;

namespace MvcSomeren.Models;

public class LecturerSupervisorViewModel
{
    public LecturerSupervisorViewModel()
    {
        Lecturers = new List<Lecturer>();
        Activities = new List<Activity>();
        Supervisor = null;
        AllLecturers = new List<Lecturer>();
        AllActivities = new List<Activity>();
        AllSupervisors = new List<Supervisor>();
    }

    public LecturerSupervisorViewModel(List<Lecturer> lecturers, List<Activity> activities, Supervisor supervisor, List<Lecturer> allLecturers, List<Activity> allActivities, List<Supervisor> allSupervisors)
    {
        Lecturers = lecturers;
        Activities = activities;
        Supervisor = supervisor;
        AllLecturers = allLecturers;
        AllActivities = allActivities;
        AllSupervisors = allSupervisors;
    }

    public List<Lecturer> Lecturers { get; set; }
    public List<Activity> Activities { get; set; }
    public Supervisor? Supervisor { get; set; }
    public List<Lecturer> AllLecturers { get; set; }
    public List<Activity> AllActivities { get; set; }
    public List<Supervisor> AllSupervisors { get; set; }
}