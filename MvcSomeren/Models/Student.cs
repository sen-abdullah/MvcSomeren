namespace MvcSomeren.Models;
using System;
public class Student
{
    //fields and properties
    public int StudentId { get; set; }
    public int StudentNumber { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public int StudentPhoneNumber { get; set; }
    public string StudentClass { get; set; }
    public int? StudentRoomId { get; set; }

    //constructors
    public Student()
    {
        StudentId = 0;
        StudentNumber = 0;
        StudentFirstName = "";
        StudentLastName = "";
        StudentPhoneNumber = 0;
        StudentClass = "";
        StudentRoomId = 0;
    }

    public Student(int studentId, int studentNumber, string studentFirstName, string studentLastName, int studentPhoneNumber, string studentClass, int? roomId)
    {
        StudentId = studentId;
        StudentNumber = studentNumber;
        StudentFirstName = studentFirstName;
        StudentLastName = studentLastName;
        StudentPhoneNumber = studentPhoneNumber;
        StudentClass = studentClass;
        StudentRoomId = roomId;
    }
}