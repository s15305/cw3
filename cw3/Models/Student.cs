using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Student
    {
        //public int IdStudnet { get; set; }
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public int IdEnrollment { get; set; }

        public string Studies { get; set; }
        public int Semester { get; set; }

        /* public DateTime StartDate{ get; set; }
         public int IdStudy { get; set; }
         public string Name { get; set; }
         */

        internal string Password;
    }
}
