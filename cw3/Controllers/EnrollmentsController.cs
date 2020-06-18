using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.DTOs.Promotion;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Services.EnrollmentDbEntityContextService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentDbEntityContextService _enrollmentService;

        public EnrollmentsController(IEnrollmentDbEntityContextService enrollmentService)
        {

            _enrollmentService = enrollmentService;

        }

        [HttpPost("promotions")]
        public IActionResult Promote(PromoteStudents promotion)
        {
            if (!_enrollmentService.EnrollmentExists(promotion.Studies, promotion.Semester))
            {

                return NotFound();

            }

            return Ok(_enrollmentService.PromoteStudent(promotion));
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest enrollment)
        {
            if (!_enrollmentService.StudiesExist(enrollment.Studies))
            {

                return BadRequest();

            }

            var enrolledStudent = _enrollmentService.EnrollStudent(enrollment);
            if (enrolledStudent == null)
            {

                return BadRequest();

            }

            return Created("Created", enrolledStudent);

        }

    }

}


/*{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private  IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
           // _dbService.EnrollStudent(request);
           // var response = new EnrollStudentResponse();
           // return Ok(response);

            return Ok(_dbService.EnrollStudent(request));
        }
    }
}*/
