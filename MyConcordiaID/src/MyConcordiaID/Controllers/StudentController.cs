using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyConcordiaID.Models;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {

        private readonly DatabaseEntities _database;

        public StudentController(IStudentRepository students, DatabaseEntities context)
        {
            Students = students;
            _database = context;
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
            var student = _database.CONCORDIAUSERS
                 .Where(s => s.ID == id)
                 .SingleOrDefault();

            if (student == null)
            {
                return NotFound();
            }
            return new ObjectResult(student);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ProfilePicture")]
        public async Task<IActionResult> PostProfilePicture(IFormFile file)
        {
            if (file == null) throw new Exception("File is null");
            if (file.Length == 0) throw new Exception("File is empty");

            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    //await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);
                }
            }

            return new StatusCodeResult(200);
        }



    }
}
