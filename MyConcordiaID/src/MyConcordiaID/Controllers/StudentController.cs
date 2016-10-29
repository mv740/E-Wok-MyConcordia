using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {

        private readonly DatabaseEntities _database;
        private IStudentRepository StudentsRepo { get; set; }

        public StudentController(IStudentRepository students, DatabaseEntities context)
        {
            StudentsRepo = students;
            _database = context;
        }
        

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return new ObjectResult(StudentsRepo.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult GetById(int id)
        {

            var student = StudentsRepo.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return new ObjectResult(student);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ProfilePicture")]
        public ActionResult PostProfilePicture(IFormFile file)
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
                    var id = StudentHelper.getRandomId();
                    PICTURE newPicture = new PICTURE { ID = id, APPROVED = null, PENDING = 1, PICTURE1 = fileContent, UPLOADEDDATE = DateTime.UtcNow };
                    picturesDb.Add(newPicture);
                    _database.SaveChanges();

                }
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ProfilePictureTest/{id}")]
        public  IActionResult PostProfilePictureTest([FromRoute]int id, IFormFile file)
        {

            if (file == null || file.Length ==0 )
                return new JsonResult(new { Error = "file is empty" }) { StatusCode = (int)HttpStatusCode.NotFound };

            StudentsRepo.AddPendingPicture(id, file);

            return Ok();
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

        [AllowAnonymous]
        [HttpGet]
        [Route("PendingPicture/{id}")]
        public IActionResult GetPendingPicture(int id)
        {

            var student = StudentsRepo.FindPendingPicture(id);

            return new ObjectResult(student);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("ValidatePicture")]
        public IActionResult PostValidatePicture([FromBody] PictureValidation picture)
        {
            StudentsRepo.ValidatePicture(picture);

            return Ok();
        }

    }
}
