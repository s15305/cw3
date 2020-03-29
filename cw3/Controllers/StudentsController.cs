using System;
using System.Collections.Generic;
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

        
        [HttpGet]

        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        /* ZAKOMENOTWANE Z POWODU ZBIERZNOŚCI NAZW

         [HttpGet]
         public string GetStudents(string orderBy)
         {
             return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
         }
         [HttpGet("{id}")]

         public IActionResult GetStudent(int id)
         {
             if (id == 1)
             {
                 return Ok("Kowalski");
             }
             else if (id == 2)
             {
                 return Ok("Malewski");
             }

             return NotFound("Nie znaleziono studenta");
         }

         [HttpPost]

         public IActionResult CreateStudent(Student student)
         {
             student.IndexNumber = $"s{new Random().Next(1, 20000)}";
             return Ok(student);
         }

         [HttpPut("{id}")]

         public IActionResult GetStudentPut(int id)
         {
             if (id == 10)
             {
                 return Ok("Aktualizacja dokończona");
             }
             else if (id == 20)
             {
                 return Ok("Aktualizacja dokończona");
             }

             return NotFound("Nie znaleziono studenta");
         }

         [HttpDelete("{id}")]
        public IActionResult GetStudentDelete(int id)
        {
            if (id == 100)
            {
                return Ok("Usuwanie ukończone");
            }
            else if (id == 200)
            {
                return Ok("Uduwanie ukończone");
            }

            return NotFound("Nie znaleziono studenta");
        }*/
    }
}
 