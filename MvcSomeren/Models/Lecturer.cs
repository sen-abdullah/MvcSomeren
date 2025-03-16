namespace MvcSomeren.Models;

public class Lecturer
{
    public int id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int Age { get; set; }

    public Lecturer()
    {
        id = 0;
        FirstName = "";
        LastName = "";
        PhoneNumber = "";
        Age = 0;
    }

    public Lecturer(int id, string firstName, string lastName, string phoneNumber, int age)
    {
        id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Age = age;
    }
}