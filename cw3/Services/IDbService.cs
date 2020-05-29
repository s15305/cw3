using cw3.DTOs.Requests;
using cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public interface IDbService
    {

         IEnumerable<Student> GetStudents();
        IEnumerable<Enrollment> GetStudent(int id);
        //5
        Enrollment EnrollStudent(EnrollStudentRequest request);
        Enrollment PromoteStudents(int semester, string studies);
        //6
        bool IsExistingStudent(string id);
        //7
        public Dictionary<string, Claim[]> getRefreshTokens();
    }
}
