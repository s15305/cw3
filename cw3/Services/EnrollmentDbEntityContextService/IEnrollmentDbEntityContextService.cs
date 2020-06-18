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

    public interface IEnrollmentDbEntityContextService
    {

        public bool EnrollmentExists(string studies, int semester);
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest enrollment);
        public Enrollment PromoteStudent(PromoteStudents promotion);
        public bool StudiesExist(string name);

    }

}