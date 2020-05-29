using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DTOs.Requests
{
    public class LoginRequest
    {
        public string Index { get; set; }
        public string Password { get; set; }
    }
}
