﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Log;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Models.Student;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace MyConcordiaID.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {

        private IStudentRepository _studentsRepo { get; set; }
        private IPictureRepository _pictureRepo { get; set; }
        private ILogRepository _logRepo { get; set; }


        public StudentController(IStudentRepository students,IPictureRepository pictures, ILogRepository logs)
        {
            _studentsRepo = students;
            _pictureRepo = pictures;
            _logRepo = logs;
        }

        /// <summary>
        /// Retrieve all the students accounts
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List of students</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        public IActionResult GetAll()
        {

            var students = _studentsRepo.GetAllAsync().Result;

            return new ObjectResult(students);
        }

        /// <summary>
        ///  Retrieve a specific student account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return student information</response>
        /// <response code="404">id is invalid, user doesn't exist</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult GetById(int id)
        {

            var student = _studentsRepo.FindByIdAsync(id).Result;

            if (student == null)
            {
                return NotFound();
            }

            return new ObjectResult(student);
        }

        /// <summary>
        ///  Retrieve all pictures related to a specific student account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return pictures</response>
        /// <response code="404">user was not found </response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("picture/{id}")]
        public IActionResult GetStudentPictures(int id)
        {

            var studentPictures = _pictureRepo.FindStudentPicturesAsync(id).Result;

            if (studentPictures == null)
            {
                return NotFound();
            }

            return new ObjectResult(studentPictures);
        }

        /// <summary>
        ///  Retrieve Authenticated user's account information
        /// </summary>
        /// <returns></returns>
        /// <remarks>Must be authenticated!</remarks>
        /// <response code="200">return account information</response>
        /// <response code="401">Not authenticated</response>
        [HttpGet]
        [Route("account")]
        public IActionResult GetAccount()
        {
            var result = _studentsRepo.FindByNetNameAsync(GetAuthenticatedUserNetname()).Result;

            if (result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        /// <summary>
        ///  Send a profile picture for validation
        /// </summary>
        /// <remarks>Must be authenticated!</remarks>
        /// <param name="file"></param>
        /// <response code="200">Picture Submited</response>
        /// <response code="400">Missing the picture or invalid</response>
        /// <response code="401">Unauthorized</response>
        [ApiExplorerSettings(IgnoreApi =true)]
        [HttpPost]
        [Route("ProfilePicture")]
        public ActionResult PostProfilePicture(IFormFile file)
        {
            //todo correctly handle error with correct http error code
            if(file == null || file.Length == 0)
            {
                //file is null or empty
                return BadRequest(); ;
            }

            //will be used to store image time
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);


            var authenticatedUser = GetAuthenticatedUserNetname();

            using (var stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);

                    _pictureRepo.AddPendingPicture(authenticatedUser, fileContent);

                }
            }

            _logRepo.LoggerAsync(authenticatedUser, Log.Action.SendPicture, null);
            
            return Ok();
        }


        /// <summary>
        ///  Add a comment about a picture
        /// </summary>
        /// <param name="comment"></param>
        /// <response code="200">Comment Submited</response>
        /// <response code="404">Missing the picture or invalid</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("comment")]
        public IActionResult PostPictureComment([FromBody] PictureComment comment)
        {
            var authenticatedUser = GetAuthenticatedUserNetname();

            var affectedUser = _pictureRepo.AddPictureComment(comment);
            if(string.IsNullOrEmpty(affectedUser))
            {
                return NotFound();
            }

            _logRepo.LoggerAsync(authenticatedUser, Log.Action.AddComment, affectedUser);

            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("PendingPicture/{id}")]
        public IActionResult GetPendingPicture(int id)
        {

            var student = _pictureRepo.FindPendingPicture(id);

            if(student == null)
            {
                return NotFound();
            }

            return new ObjectResult(student);
        }

        /// <summary>
        ///  Validate a pending picture
        ///  if approved : student's pending picture become his valid profile picture
        ///  if denied : pending picture is send to archive picture 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns>student netname</returns>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("ValidatePicture")]
        public IActionResult PostValidatePicture([FromBody] PictureValidation picture)
        {
            var authenticatedUser = GetAuthenticatedUserNetname();

            var netName = _studentsRepo.ValidatePicture(picture, authenticatedUser);

            _logRepo.LoggerAsync(authenticatedUser, picture.Valid ? Log.Action.ApprovePicture : Log.Action.DeniedPicture,
                netName);


            return Ok();
        }


        /// <summary>
        /// Reapprove a previous picture 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns>student netname</returns>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("RevalidatePicture")]
        public IActionResult PostRevalidatePicture([FromBody] PictureValidation picture)
        {
             var authenticatedUser = GetAuthenticatedUserNetname();

            var netName = _studentsRepo.RevalidatePicture(picture, authenticatedUser);

            _logRepo.LoggerAsync(authenticatedUser, picture.Valid ? Log.Action.ReApprovedPicture : Log.Action.DeniedPicture,
                netName);


            return Ok();
        }


        /// <summary>
        ///  Retrive the current update picture period for this academic year
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("UpdatePeriod")]
        public IActionResult GetUpdatePicturePeriod()
        {
            var updatePeriod = _studentsRepo.GetUpdatePicturePeriodAsync().Result;

            return new ObjectResult(updatePeriod);
        }

        /// <summary>
        ///  Search for specific student using different parameter : netname, date, id, first name, last name
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("Search")]
        public IActionResult SearchByParameters([FromBody] SearchOptions searchOptions)
        {
            var result = _studentsRepo.Search(searchOptions);
            if(!result.Any())
            {
                return NotFound();
            }
            return new ObjectResult(result);   
        }

        private string GetAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName); 
            var lastName = User.FindFirstValue(ClaimTypes.Surname); 
            return StudentHelper.GenerateNetName(firstName, lastName);
        }

    }
}
