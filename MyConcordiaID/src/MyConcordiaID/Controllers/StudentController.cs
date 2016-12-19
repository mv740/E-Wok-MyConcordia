using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Log;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {

        private readonly DatabaseEntities _database;
        private IStudentRepository _studentsRepo { get; set; }
        private ILogRepository _logRepo { get; set; }


        public StudentController(IStudentRepository students, ILogRepository logs, DatabaseEntities context)
        {
            _studentsRepo = students;
            _logRepo = logs;
            _database = context;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return new ObjectResult(_studentsRepo.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult GetById(int id)
        {

            var student = _studentsRepo.FindById(id);

            if (student == null)
            {
                return NotFound();
            }

            return new ObjectResult(student);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("picture/{id}")]
        public IActionResult GetStudentPictures(int id)
        {

            var studentPictures = _studentsRepo.FindStudentPictures(id);

            if (studentPictures == null)
            {
                return NotFound();
            }

            return new ObjectResult(studentPictures);
        }


        [Authorize]
        [HttpGet]
        [Route("account")]
        public IActionResult GetAccount()
        {
            var result = _studentsRepo.FindByNetName(getAuthenticatedUserNetname());

            return new ObjectResult(result);
        }


        [Authorize]
        [HttpPost]
        [Route("ProfilePicture")]
        public ActionResult PostProfilePicture(IFormFile file)
        {
            //todo correctly handle error with correct http error code

            if (file == null) throw new Exception("File is null");
            if (file.Length == 0) throw new Exception("File is empty");

            //will be used to store image time
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);


            var authenticatedUser = getAuthenticatedUserNetname();

            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);

                    _studentsRepo.AddPendingPicture(authenticatedUser, fileContent);



                }
            }

            _logRepo.Logger(authenticatedUser, Log.Action.SendPicture);
            


            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Pending")]
        public IActionResult GetAllPending()
        {
            return new ObjectResult(_studentsRepo.GetAllPending());
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("PendingPicture/{id}")]
        public IActionResult GetPendingPicture(int id)
        {

            var student = _studentsRepo.FindPendingPicture(id);

            return new ObjectResult(student);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ValidatePicture")]
        public IActionResult PostValidatePicture([FromBody] PictureValidation picture)
        {
            //var authenticatedUser = getAuthenticatedUserNetname();

            _studentsRepo.ValidatePicture(picture);

            //if(picture.valid)
            //{
            //    _logRepo.Logger(authenticatedUser, Log.Action.ApprovePicture);
            //}
            //else
            //{
            //    _logRepo.Logger(authenticatedUser, Log.Action.DeniedPicture);
            //}


            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Valid")]
        public IActionResult GetAllValid()
        {
            return new ObjectResult(_studentsRepo.GetAllValid());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UpdatePeriod")]
        public IActionResult GetUpdatePicturePeriod()
        {
            return new ObjectResult(_studentsRepo.GetUpdatePicturePeriod());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Search")]
        public IActionResult SearchByParameters([FromBody] SearchOptions searchOptions)
        {
            return new ObjectResult(_studentsRepo.Search(searchOptions));
            
        }


        
        public string getAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName); 
            var lastName = User.FindFirstValue(ClaimTypes.Surname); 
            return StudentHelper.GenerateNetName(firstName, lastName);
        }



    }
}
