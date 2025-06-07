using MvcSomeren.Repositories;

namespace MvcSomeren;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IDrinkOrderRepository, DbDrinkOrderRepository>();
        builder.Services.AddSingleton<IDrinksRepository, DbDrinksRepository>();
        
        var _student_rep = new DbStudentRepository(builder.Configuration);
        builder.Services.AddSingleton<IStudentRapository>(_student_rep);
        CommonRepository._studentRapository = _student_rep;

        var _participant_rep = new DbParticipantRepository(builder.Configuration);
        builder.Services.AddSingleton<IParticipantsRepository>(_participant_rep);
        CommonRepository._participantsRepository = _participant_rep;

        var _lecturer_rep = new DbLecturersRepository(builder.Configuration);
        builder.Services.AddSingleton<ILecturersRepository>(_lecturer_rep);
        CommonRepository._lecturersRepository = _lecturer_rep;
        
        var _supervisor_rep = new DbSupervisorRepository(builder.Configuration);
        builder.Services.AddSingleton<ISupervisorRepository>(_supervisor_rep);
        CommonRepository._supervisorRepository = _supervisor_rep;
        
        var _rooms_rep = new DbRoomsRepository(builder.Configuration);
        builder.Services.AddSingleton<IRoomsRepository>(_rooms_rep);
        CommonRepository._roomsRepository = _rooms_rep;
        
        var _lecturer_supervisor = new DbLecturerSupervisorRepository(builder.Configuration);
        builder.Services.AddSingleton<ILecturerSupervisorRepository>(_lecturer_supervisor);
        CommonRepository._lecturerSupervisorRepository = _lecturer_supervisor;
        
        var _activity_rep = new DbActivitiesRepository(builder.Configuration);
        builder.Services.AddSingleton<IActivitiesRepository>(_activity_rep);
        CommonRepository._activitiesRepository = _activity_rep;
        
        var _manage_participants_rep= new DbManageParticipantsRepository(builder.Configuration);
        builder.Services.AddSingleton<IManageParticipantsRepository>(_manage_participants_rep);
        CommonRepository._manageParticipantsRepository = _manage_participants_rep;


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}