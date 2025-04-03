using MvcSomeren.Repositories;

namespace MvcSomeren;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IStudentRapository, DbStudentRepository>();
        builder.Services.AddSingleton<ILecturersRepository, DbLecturersRepository>();
        builder.Services.AddSingleton<IRoomsRepository, DbRoomsRepository>();
        builder.Services.AddSingleton<ISupervisorRepository, DbSupervisorRepository>();
        builder.Services.AddSingleton<IActivitiesRepository, DbActivitiesRepository>();
        builder.Services.AddSingleton<IDrinkOrderRepository, DbDrinkOrderRepository>();

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