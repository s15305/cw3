using System.Collections.Generic;
using cw3.Models;

namespace cw3.Services.StudentDbEntityContextService
{

    public interface IStudentDbEntityContextService
    {

        public IEnumerable<Student> GetStudents();
        public Student ModifyStudent(Student updated);
        public Student DeleteStudent(string index);
        public Student AddStudent(Student student);

    }

}
