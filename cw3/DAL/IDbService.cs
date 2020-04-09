using cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public interface IDbService
    {

        IEnumerable<Student> GetStudents();
        IEnumerable<Enrollment> GetStudent(int id);
    }
}
