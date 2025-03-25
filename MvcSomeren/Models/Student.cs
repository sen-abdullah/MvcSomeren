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
    
    public bool Voucher { get; set; }
    public int? StudentRoomId { get; set; }


    //constructors
    public Student()
    {
        this.StudentId = 0;
        this.StudentNumber = 0;
        this.StudentFirstName = "";
        this.StudentLastName = "";
        this.StudentPhoneNumber = 0;
        this.StudentClass = "";
        this.Voucher = false;
        this.StudentRoomId = 0;
    }

    public Student(int studentId, int studentNumber, string studentFirstName, string studentLastName, int studentPhoneNumber, string studentClass, bool voucher, int? roomId)
    {
        this.StudentId = studentId;
        this.StudentNumber = studentNumber;
        this.StudentFirstName = studentFirstName;
        this.StudentLastName = studentLastName;
        this.StudentPhoneNumber = studentPhoneNumber;
        this.StudentClass = studentClass;
        this.Voucher = voucher;
        this.StudentRoomId = roomId;
    }
}