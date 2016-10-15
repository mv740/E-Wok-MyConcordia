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
using System.Net.Mime;
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

            //will be used to store image time
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
         
            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    //await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);
                    var picturesDb = _database.Set<PICTURE>();
                    Random rnd = new Random();
                    var id  = rnd.Next(20000000, 99999999);
                    PICTURE newPicture = new PICTURE { ID = id, APPROVED = null, PENDING = 1, PICTURE1 = fileContent, UPLOADEDDATE = DateTime.UtcNow };
                    picturesDb.Add(newPicture);
                    _database.SaveChanges();

                }
            }
           
            return new StatusCodeResult(200);
        }

        /// <summary>
        /// Get lastest uploaed picture
        /// 
        /// Will later be changed, to show user profile picture
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ProfilePicture")]
        public FileStreamResult GetProfilePicture()
        {
            var picture = _database.PICTUREs.OrderByDescending(p => p.UPLOADEDDATE).First();
            Stream stream = new MemoryStream(picture.PICTURE1);
            return new FileStreamResult(stream, new MediaTypeHeaderValue("image/png"));
        }



    }
}
