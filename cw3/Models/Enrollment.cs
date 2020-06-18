using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public partial class Enrollment
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Studies IdStudyNavigation { get; set; }
        public virtual ICollection<Student> Student { get; set; }

        public Enrollment()
        {
            Student = new HashSet<Student>();
        }


    }
}
