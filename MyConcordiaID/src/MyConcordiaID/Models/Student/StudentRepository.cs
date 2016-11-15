using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OracleEntityFramework;
using System.IO;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Helper;

namespace MyConcordiaID.Models.Student
{
    public class StudentRepository : IStudentRepository
    {

        private readonly DatabaseEntities _database;

        public StudentRepository(DatabaseEntities context)
        {
            _database = context;
        }

        public STUDENT Find(int id)
        {
            var student = _database.STUDENTS
                 .Where(s => s.ID == id)
                 .SingleOrDefault();

            return student;
        }

        public dynamic GetAll()
        {

            var students = _database.STUDENTS.Select(s => new { s.ID, s.NETNAME, s.FIRSTNAME, s.LASTNAME });

            return students;
        }

        public void AddPendingPicture(int id, IFormFile file)
        {

            //will be used to store image time
            //var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);

                    var student = _database.STUDENTS
                        .Where(s => s.ID == id)
                        .FirstOrDefault();

                    student.PENDINGPICTURE = fileContent;
                    student.PENDING = true;
                    student.UPDATEPICTURE = false; // so he can't send multiple update 
                    _database.SaveChanges();


                }
            }
            
        }

        public dynamic FindPendingPicture(int id)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == id)
                .Select(s => new { s.ID, s.PENDINGPICTURE, s.PREVIOUSPICTURE1, s.PREVIOUSPICTURE2 })
                .FirstOrDefault();

            return student;
            
        }

        public void ValidatePicture(PictureValidation pictureValidation)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == pictureValidation.id)
                .FirstOrDefault();

           
            if(pictureValidation.valid)
            {

                if(student.PROFILEPICTURE !=null)
                {
                    if(student.PREVIOUSPICTURE1 != null)
                    {
                        var previous1 = student.PREVIOUSPICTURE1;
                        student.PREVIOUSPICTURE2 = previous1;
                    }

                    var previous = student.PROFILEPICTURE;
                    student.PREVIOUSPICTURE1 = previous; 
                }

                byte[] validPicture = student.PENDINGPICTURE;
                student.PROFILEPICTURE = validPicture;
                student.PENDINGPICTURE = null;
                student.VALID = true;

            }
            else
            {
                student.PENDINGPICTURE = null;
            }
            student.PENDING = false;
            _database.SaveChanges();

        }

        public dynamic GetAllPending()
        {

           var students =  _database.STUDENTS
                .Where(s => s.PENDING == true)
                .Select(s=> new { s.ID, s.NETNAME, s.FIRSTNAME, s.LASTNAME})
                .ToList();

            return students;

           
        }

        public dynamic GetAllValid()
        {

            var students = _database.STUDENTS
                 .Where(s => s.VALID == true)
                 .Select(s => new { s.ID, s.NETNAME, s.FIRSTNAME, s.LASTNAME })
                 .ToList();

            return students;


        }

        public void Add(STUDENT newStudent)
        {
            _database.STUDENTS.Add(newStudent);
            _database.SaveChanges();
        }

        public PicturePeriod GetUpdatePicturePeriod()
        {
            // May 1st 2016 start of academic year 2016-17 : summer 2016, fall 16, winter 17

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;


            int academicYear;
            if(month >= 5 )
            {
                academicYear = year;
            }
            else
            {
                academicYear = year - 1;
            }

            var period = _database.PICTUREUPDATESETTINGs
                .Where(p => p.YEAR == academicYear)
                .FirstOrDefault();

            DateTime today = DateTime.Today;


            bool canUpdate = false;
            if(today >= period.STARDATE && today <= period.ENDDATE)
            {
                canUpdate = true;
            }

            PicturePeriod picturePeriod = new PicturePeriod
            {
                canUpdatePicture = canUpdate,
                startDate = period.STARDATE.ToString("dd-MM-yyyy"),
                endDate = period.ENDDATE.ToString("dd-MM-yyyy")
            };

            return picturePeriod;


        }

        public List<STUDENT> Search(SearchOptions searchOptions)
        {


            IQueryable<STUDENT> student = _database.STUDENTS;

            //each if statement will try to build the where clauses and it only be executed when ToList() is called 
            if(searchOptions.id.HasValue)
            {
                if(StudentHelper.ValidId(searchOptions.id.Value))
                {
                    student = student.Where(s => s.ID == searchOptions.id.Value);
                }
            }
            if (!string.IsNullOrEmpty(searchOptions.name))
            {
                student = student.Where(s => s.FIRSTNAME.Contains(searchOptions.name) || s.LASTNAME.Contains(searchOptions.name));
            }
            if(!string.IsNullOrEmpty(searchOptions.birthdate))
            {
                //parse dob 
                //add linq query
            }
            if(!string.IsNullOrEmpty(searchOptions.netname))
            {
                student = student.Where(s => s.NETNAME.Contains(searchOptions.netname));
            }

           return  student.ToList();
            
        }
    }

}
