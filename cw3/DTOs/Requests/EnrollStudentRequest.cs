using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }
        //public string Email { get; set; }

        [Required/*(ErrorMessage = "Musisz podać imię")*/]
       // [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
       // [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Studies { get; set; }
    }
}
