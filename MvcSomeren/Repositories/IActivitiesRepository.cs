using MvcSomeren.Models;

namespace MvcSomeren.Repositories;

public interface IActivitiesRepository
{
    List<Activity> GetAll();
    Activity? GetById(int activityId);
    void Add(Activity activity);
    void Update(Activity activity);
    List<Activity> Filter(string activityName);
    void Delete(Activity activity);
}