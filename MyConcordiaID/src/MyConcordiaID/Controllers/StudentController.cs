using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        public StudentController(IStudentRepository students)
        {
            Students = students;
        }
        public IStudentRepository Students { get; set; }

        
        [HttpGet]
        public IEnumerable<Student> GetAll()
        {
            return Students.GetAll();
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult GetById(int id)
        {
            var item = Students.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
    }
}
