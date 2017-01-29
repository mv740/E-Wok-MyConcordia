using System;
using System.Collections.Generic;
using System.Linq;
using OracleEntityFramework;
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

        public StudentAccount FindById(int id)
        {
            var student = _database.STUDENTS
                 .Where(s => s.ID == id)
                 .Select(s => new
                 {
                     s.NETNAME,
                     s.ID,
                     s.FIRSTNAME,
                     s.LASTNAME,
                     s.DOB,
                     s.VALID,
                     s.PENDING,
                     s.UGRADSTATUS,
                     s.EXPIREDATE,
                     s.UPDATEPICTURE
                 })
                 .SingleOrDefault();

            if (student != null)
            {
                StudentAccount account = new StudentAccount
                {
                    ID = student.ID,
                    NetName = student.NETNAME,
                    FirstName = student.FIRSTNAME,
                    LastName = student.LASTNAME,
                    DOB = student.DOB,
                    Valid = student.VALID,
                    Pending = student.PENDING,
                    UGradStatus = student.UGRADSTATUS,
                    ExpireDate = student.EXPIREDATE
                };

                if (student.PENDING)
                {
                    //retrieve pending picture
                    string pending = Status.Pending.ToString();
                    var pendingPicture = _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == pending)
                        .FirstOrDefault();

                    account.PendingPicture = pendingPicture.PICTURE_DATA;

                }

                if (student.VALID)
                {
                    //retrieve profile  picture
                    string aproved = Status.Approved.ToString();
                    var profilePicture = _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == aproved)
                        .FirstOrDefault();

                    account.ProfilePicture = profilePicture.PICTURE_DATA;

                }

                return account;
            }

            //not found
            return null;
        }

        public StudentAccount FindByNetName(string netName)
        {
            var student = _database.STUDENTS
                 .Where(s => s.NETNAME == netName)
                 .Select(s => new
                 {
                     s.NETNAME,
                     s.ID,
                     s.FIRSTNAME,
                     s.LASTNAME,
                     s.DOB,
                     s.VALID,
                     s.PENDING,
                     s.UGRADSTATUS,
                     s.EXPIREDATE,
                     s.UPDATEPICTURE
                 })
                 .SingleOrDefault();

            if(student != null)
            {
                StudentAccount account = new StudentAccount
                {
                    ID = student.ID,
                    NetName = student.NETNAME,
                    FirstName = student.FIRSTNAME,
                    LastName = student.LASTNAME,
                    DOB = student.DOB,
                    Valid = student.VALID,
                    Pending = student.PENDING,
                    UGradStatus = student.UGRADSTATUS,
                    ExpireDate = student.EXPIREDATE,
                    UpdatePicture = student.UPDATEPICTURE
                };

                if (student.PENDING)
                {
                    //retrieve pending picture
                    string pending = Status.Pending.ToString();
                    var pendingPicture = _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == pending)
                        .FirstOrDefault();

                    account.PendingPicture = pendingPicture.PICTURE_DATA;

                }

                if (student.VALID)
                {
                    //retrieve profile  picture
                    var approved = Status.Approved.ToString();
                    var profilePicture = _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == approved)
                        .FirstOrDefault();

                    account.ProfilePicture = profilePicture.PICTURE_DATA;

                }

                return account;
            }

            //not found
            return null;
        }

        /// <summary>
        ///  Retrieve all the students basic informations
        /// </summary>
        /// <returns></returns>
        public dynamic GetAll()
        {

            var students = _database.STUDENTS
                .Select(s => new
                {
                    s.ID,
                    s.NETNAME,
                    s.FIRSTNAME,
                    s.LASTNAME,
                    s.DOB
                })
                .ToList();

            return students;
        }

        /// <summary>
        ///  Validate a pending picture 
        ///  If approved, the student pending picture become their valid profile picture
        ///  if denied, the pending picture is send to the picture archive
        /// </summary>
        /// <param name="pictureValidation">id : student id,valid : bool</param>
        /// <returns>student netname</returns>
        public string ValidatePicture(PictureValidation pictureValidation,string netName)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == pictureValidation.id)
                .FirstOrDefault();

            var studentNetname = student.NETNAME;


            var pending = Status.Pending.ToString();
            var pendingPicture = _database.PICTUREs.
                Where(p => p.STATUS == pending && p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

            if(pictureValidation.valid)
            {
                pendingPicture.STATUS = Status.Approved.ToString();

                //if user has already a profile picture, set to archive
                var aprouved = Status.Approved.ToString();
                var profilePicture = _database.PICTUREs.
                Where(p => p.STATUS == aprouved && p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

                if(profilePicture != null)
                {
                    profilePicture.STATUS = Status.Archived.ToString();
                    profilePicture.UPDATED = DateTime.UtcNow;
                    profilePicture.ADMINISTRATOR = netName;
                }

                //account is now valid
                student.VALID = true;

            }
            else
            {
                pendingPicture.STATUS = Status.Denied.ToString();
            }


            student.PENDING = false;
            //log the admin doing the validation
            pendingPicture.ADMINISTRATOR = netName;
            pendingPicture.UPDATED = DateTime.UtcNow;
            _database.SaveChanges();

            return studentNetname;
        }

        /// <summary>
        ///  Approve once more a previous picture or invalidate the current profile picture
        /// </summary>
        /// <param name="pictureValidation">id : picture id, valid : bool </param>
        /// <returns>student netname</returns>
        public string RevalidatePicture(PictureValidation pictureValidation, string netName)
        {

            //find selected picture
            var selectedPicture = _database.PICTUREs
                 .Where(p => p.ID_PK == pictureValidation.id)
                 .FirstOrDefault();

            var studentNetname = selectedPicture.STUDENT_NETNAME;

            //find valid profile picture if user has one 
            var approved = Status.Approved.ToString();
            var currentProfilePicture = _database.PICTUREs
                .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == approved)
                .FirstOrDefault();


            if (pictureValidation.valid)
            {
                if(currentProfilePicture !=null)
                {
                    //archived picture
                    currentProfilePicture.STATUS = Status.Archived.ToString();
                }

                //new profile
                selectedPicture.STATUS = Status.Approved.ToString();
                selectedPicture.UPDATED = DateTime.UtcNow;
                selectedPicture.ADMINISTRATOR = netName;

                //find his account and set it to valid
                var student = _database.STUDENTS
                    .Where(s => s.NETNAME == studentNetname)
                    .FirstOrDefault();

                student.VALID = true;



            }
            else
            {

                //invalidate current profile picture
                currentProfilePicture.STATUS = Status.Denied.ToString();

                //find his account and set it to invalid
                var student = _database.STUDENTS
                    .Where(s => s.NETNAME == currentProfilePicture.STUDENT_NETNAME)
                    .FirstOrDefault();

                student.VALID = false;
                  
            }
            //log admin
            if(currentProfilePicture != null)
            {
                currentProfilePicture.UPDATED = DateTime.UtcNow;
                currentProfilePicture.ADMINISTRATOR = netName;
            }


            _database.SaveChanges();

            return studentNetname;
        }

        /// <summary>
        ///  Registered student to database
        /// </summary>
        /// <param name="newStudent"></param>
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
            if (month >= 5)
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

            if(period != null)
            {
                DateTime today = DateTime.Today;

                bool canUpdate = false;
                if (today >= period.STARDATE && today <= period.ENDDATE)
                {
                    canUpdate = true;
                }

                PicturePeriod picturePeriod = new PicturePeriod
                {
                    canUpdatePicture = canUpdate,
                    startDate = period.STARDATE.ToString(),
                    endDate = period.ENDDATE.ToString()
                };

                return picturePeriod;
            }
            else
            {
                PicturePeriod picturePeriod = new PicturePeriod
                {
                    canUpdatePicture = false,
                    startDate = string.Empty,
                    endDate = string.Empty
                };
                return picturePeriod;
            }

        }

        /// <summary>
        ///  Search for specific user with parameters 
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        public IEnumerable<STUDENT> Search(SearchOptions searchOptions)
        {

            //empty 
            if(string.IsNullOrEmpty(searchOptions.birthdate) && !searchOptions.id.HasValue && searchOptions.name.Capacity == 0 && string.IsNullOrEmpty(searchOptions.netname))
            {
                return Enumerable.Empty<STUDENT>();
            }

            //else search 
            IQueryable<STUDENT> student = _database.STUDENTS;

            //each if statement will try to build the where clauses and it only be executed when ToList() is called 
            if (searchOptions.id.HasValue)
            {
                if (StudentHelper.ValidId(searchOptions.id.Value))
                {
                    student = student.Where(s => s.ID == searchOptions.id.Value);
                }
            }
            if (searchOptions.name.Count != 0)
            {
                foreach (var name in searchOptions.name)
                {
                    student = student.Where(s => s.FIRSTNAME.Contains(name.ToLower()) || s.LASTNAME.Contains(name.ToLower()));
                }
            }
            if (!string.IsNullOrEmpty(searchOptions.birthdate))
            {
                DateTime dateValue;
                DateTime.TryParse(searchOptions.birthdate, out dateValue);

                student = student.Where(s => s.DOB == dateValue);
            }
            if (!string.IsNullOrEmpty(searchOptions.netname))
            {
                student = student.Where(s => s.NETNAME.Contains(searchOptions.netname));
            }

            return student.ToList();

        }

        public bool DoesStudentExist(string firstName, string lastName)
        {

            var student = _database.STUDENTS
                .Where(s => s.FIRSTNAME == firstName.ToLower() && s.LASTNAME == lastName.ToLower())
                .FirstOrDefault();

            return (student == null) ? false : true;

        }
    }

}
