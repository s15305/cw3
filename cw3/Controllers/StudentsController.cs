using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        
         private readonly IDbService _dbService;

        public StudentsController (IDbService dbService)
        {
            _dbService = dbService;
        }

       /* stare
        [HttpGet]

        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("enrollment/{numerIndeksu}")]
        public IActionResult GetEnrollmentForStudent(int numerIndeksu)
        {
            return Ok(_dbService.GetStudent(numerIndeksu));
        }*/
        [HttpPost]

         public IActionResult CreateStudent(Student student)
         {
             student.IndexNumber = $"s{new Random().Next(1, 20000)}";
             return Ok(student);
         }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id)
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }
    }
}
 