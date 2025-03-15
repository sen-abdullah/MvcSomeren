namespace MvcSomeren.Models;
using System;
public class Student
{
    //fields and properties
    public int StudentNumber { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public string StudentPhoneNumber { get; set; }
    public string StudentClass { get; set; }


    //constructors
    public Student()
    {
        this.StudentNumber = 0;
        this.StudentFirstName = "";
        this.StudentLastName = "";
        this.StudentPhoneNumber = "";
        this.StudentClass = "";
    }

    public Student(int studentNumber, string studentFirstName, string studentLastName, string studentPhoneNumber, string studentClass)
    {
        this.StudentNumber = studentNumber;
        this.StudentFirstName = studentFirstName;
        this.StudentLastName = studentLastName;
        this.StudentPhoneNumber = studentPhoneNumber;
        this.StudentClass = studentClass;
    }
}