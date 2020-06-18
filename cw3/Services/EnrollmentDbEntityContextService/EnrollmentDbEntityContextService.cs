using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DTOs.Promotion;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;

namespace cw3.Services.EnrollmentDbEntityContextService
{

    public class EnrollmentDbEntityContextService : IEnrollmentDbEntityContextService
    {

        private readonly s15305Context _context;

        public EnrollmentDbEntityContextService(s15305Context context)
        {

            _context = context;

        }

        public bool StudiesExist(string name)
        {

            return _context.Studies.Count(s => s.Name == name) > 0;

        }

        public Enrollment PromoteStudent(PromoteStudents promotion)
        {

            var idStudy = _context.Studies.FirstOrDefault(s => s.Name == promotion.Studies).IdStudy;

            var currentEnrollment = _context.Enrollment
                .FirstOrDefault(e =>
                    e.IdStudy == idStudy &&
                    e.Semester == promotion.Semester

                );

            var nextEnrollment = _context.Enrollment
                .FirstOrDefault(e =>
                    e.IdStudy == idStudy &&
                    e.Semester == promotion.Semester + 1

                );


            if (nextEnrollment == null)
            {

                nextEnrollment = new Enrollment
                {

                    IdEnrollment = _context.Enrollment.Max(e => e.IdEnrollment) + 1,
                    Semester = promotion.Semester + 1,
                    IdStudy = idStudy,

                };

                _context.Enrollment.Add(nextEnrollment);
                _context.SaveChanges();

            }


            var enrolledStudents = _context.Student.Where(s =>
                s.IdEnrollment == currentEnrollment.IdEnrollment
            ).ToList();

            enrolledStudents.ForEach(s => { s.IdEnrollment = nextEnrollment.IdEnrollment; });

            _context.SaveChanges();
            return nextEnrollment;

        }

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest enrollment)
        {

            var enrollmentStudyId = _context.Studies.First(st => st.Name == enrollment.Studies).IdStudy;
            var recentEnrollment = _context.Enrollment
                .Where(enr => enr.Semester == 1 && enr.IdStudy == enrollmentStudyId)
                .OrderByDescending(enr => enr.IdEnrollment)
                .FirstOrDefault();


            if (recentEnrollment == null)
            {

                recentEnrollment = _context.Enrollment.Add(new Enrollment
                {

                    IdStudy = enrollmentStudyId,
                    StartDate = DateTime.Now,
                    Semester = 1,
                    Student = new HashSet<Student>()
                }).Entity;

            }

            _context.Student.Add(new Student
            {

                IndexNumber = enrollment.IndexNumber,
                FirstName = enrollment.FirstName,
                LastName = enrollment.LastName,
                BirthDate = enrollment.BirthDate,
                IdEnrollment = recentEnrollment.IdEnrollment,

            });

            _context.SaveChanges();
            return new EnrollStudentResponse
            {

                IdStudies = recentEnrollment.IdStudy,
                IdEnrollment = recentEnrollment.IdEnrollment,
                Semester = recentEnrollment.Semester,
                StartDate = recentEnrollment.StartDate,

            };
        }

        public bool EnrollmentExists(string studies, int semester)
        {

            return _context.Enrollment.Count(e =>
                e.IdStudy == _context.Studies.First(s => s.Name == studies).IdStudy &&
                e.Semester == semester) > 0;

        }
    }
}
