using Microsoft.AspNetCore.Mvc;

namespace MvcSomeren.Models
{
    public class ManageParticipantViewModel 
    {
        public List<Student> Students { get; set; }
        public List<Activity> Activities { get; set; }
        public Participator? Participator { get; set; }
        public List<Student> AllStudents { get; set; }
        public List<Activity> AllActivities { get; set; }
        public List<Participator> AllParticipators { get; set; }
        public ManageParticipantViewModel()
        {
            Students = new List<Student>();
            Activities = new List<Activity>();
            Participator = null;
            AllStudents = new List<Student>();
            AllActivities = new List<Activity>();
            AllParticipators = new List<Participator>();
        }

        public ManageParticipantViewModel(List<Student> students, List<Activity> activities, Participator participator, List<Student> allStudents, List<Activity> allActivities, List<Participator> allParticipators)
        {
            Students = students;
            Activities = activities;
            Participator = participator;
            AllStudents = allStudents;
            AllActivities = allActivities;
            AllParticipators = allParticipators;
        }
    }
}
