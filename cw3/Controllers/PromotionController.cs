using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.DTOs.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [Route("api/enrollments/promotions")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private  IDbService _dbService;


        public PromotionController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult PromotionStudents(PromoteStudents request)
        {

            return Ok(_dbService.PromoteStudents(request.Semester, request.Studies));

        }

    }
}
