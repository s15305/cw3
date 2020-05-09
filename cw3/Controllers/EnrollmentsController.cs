using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
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
}