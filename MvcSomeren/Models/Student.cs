namespace MvcSomeren.Models;
using System;
public class Student
{
    //fields and properties
    public int StudentNumber { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public int StudentPhoneNumber { get; set; }
    public string StudentClass { get; set; }
    
    public bool Voucher { get; set; }
    public int RoomId { get; set; }


    //constructors
    public Student()
    {
        this.StudentNumber = 0;
        this.StudentFirstName = "";
        this.StudentLastName = "";
        this.StudentPhoneNumber = 0;
        this.StudentClass = "";
        this.Voucher = false;
        this.RoomId = 0;
    }

    public Student(int studentNumber, string studentFirstName, string studentLastName, int studentPhoneNumber, string studentClass, bool voucher, int roomId)
    {
        this.StudentNumber = studentNumber;
        this.StudentFirstName = studentFirstName;
        this.StudentLastName = studentLastName;
        this.StudentPhoneNumber = studentPhoneNumber;
        this.StudentClass = studentClass;
        this.Voucher = voucher;
        this.RoomId = roomId;
    }
}