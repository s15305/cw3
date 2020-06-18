using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.Models;

namespace cw3.Services.StudentDbEntityContextService
{
    public class StudentDbEntityContextService : IStudentDbEntityContextService
    {

        private s15305Context _context;
        public StudentDbEntityContextService(s15305Context context)
        {

            _context = context;

        }

        public IEnumerable<Student> GetStudents()
        {

            return _context.Student.ToList();

        }

        public Student ModifyStudent(Student updated)
        {

            var result = _context.Update(updated).Entity;
            _context.SaveChangesAsync();
            return result;

        }

        public Student DeleteStudent(string index)
        {

            var student = new Student { IndexNumber = index };
            _context.Attach(student);
            var response = _context.Remove(student).Entity;
            _context.SaveChanges();
            return response;

        }

        public Student AddStudent(Student student)
        {

            var result = _context.Student.Add(student).Entity;
            _context.SaveChanges();
            return result;

        }

    }
}
