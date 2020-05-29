using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using cw3.DTOs.Requests;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly IDataProtectionProvider _provider;
        public StudentsController (IDbService dbService, IDataProtectionProvider provider)
        {
            _dbService = dbService;
            _provider = provider;
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
        [HttpPost("login")]
        public IActionResult Login(LoginRequest login)
        {
            var student = _dbService.GetStudents().First(s => s.IndexNumber == login.Index);
            var protector = _provider.CreateProtector(Environment.GetEnvironmentVariable("PASSWORD_SECRET"));

            if (student == null || login.Password != protector.Unprotect(student.Password))
            {
                return BadRequest();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "s1"),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "student"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT__KEY")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "https://localhost",
                "https://localhost",
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            var refreshToken = Guid.NewGuid().ToString();
            _dbService.getRefreshTokens().Add(refreshToken, claims);

            return Ok(new
            {
                token,
                refreshToken
            });

        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            var tokens = _dbService.getRefreshTokens();
            if (!tokens.ContainsKey(refreshToken))
            {
                return BadRequest();
            }
            var claims = tokens[refreshToken];
            tokens.Remove(refreshToken);
            var secret = Environment.GetEnvironmentVariable("JWT__KEY");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "https://localhost",
                "https://localhost",
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var newRefreshToken = Guid.NewGuid().ToString();
            tokens.Add(newRefreshToken, claims);
            return Ok(new
            {
                token,
                refreshToken = newRefreshToken
            });
        }
    }
}