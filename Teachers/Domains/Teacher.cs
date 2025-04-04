using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teachers.Domains
{
    public class Teacher
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentId { get; set; }
        public Teacher()
        {
            
        }

        public Teacher(int id, string lastName, string firstName, DateTime birthDate, int departmentId)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            BirthDate = birthDate;
            DepartmentId = departmentId;
        }
    }
}
