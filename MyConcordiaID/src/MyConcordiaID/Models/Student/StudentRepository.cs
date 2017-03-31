using System;
using System.Collections.Generic;
using System.Linq;
using OracleEntityFramework;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Helper;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MyConcordiaID.Models.Student
{
    public class StudentRepository : IStudentRepository
    {

        private readonly DatabaseEntities _database;

        public StudentRepository(DatabaseEntities context)
        {
            _database = context;
        }

        public async Task<StudentAccount> FindByIdAsync(int id)
        {
            var student = await _database.STUDENTS
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
                 .SingleOrDefaultAsync();

            if (student != null)
            {
                StudentAccount account = new StudentAccount
                {
                    Id = student.ID,
                    NetName = student.NETNAME,
                    FirstName = student.FIRSTNAME,
                    LastName = student.LASTNAME,
                    Dob = student.DOB,
                    Valid = student.VALID,
                    Pending = student.PENDING,
                    UGradStatus = student.UGRADSTATUS,
                    ExpireDate = student.EXPIREDATE
                };

                if (student.PENDING)
                {
                    //retrieve pending picture
                    string pending = Status.Pending.ToString();
                    var pendingPicture = await _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == pending)
                        .FirstOrDefaultAsync();

                    account.PendingPicture = pendingPicture.PICTURE_DATA;

                }

                if (student.VALID)
                {
                    //retrieve profile  picture
                    string aproved = Status.Approved.ToString();
                    var profilePicture = await _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == aproved)
                        .FirstOrDefaultAsync();

                    account.ProfilePicture = profilePicture.PICTURE_DATA;

                }

                return account;
            }

            //not found
            return null;
        }

        public async Task<StudentAccount> FindByNetNameAsync(string netName)
        {
            var student = await _database.STUDENTS
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
                 .SingleOrDefaultAsync();

            
            if(student != null)
            {
                StudentAccount account = new StudentAccount
                {
                    Id = student.ID,
                    NetName = student.NETNAME,
                    FirstName = student.FIRSTNAME,
                    LastName = student.LASTNAME,
                    Dob = student.DOB,
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
                    var pendingPicture = await _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == pending)
                        .FirstOrDefaultAsync();

                    account.PendingPicture = pendingPicture.PICTURE_DATA;

                }

                if (student.VALID)
                {
                    //retrieve profile  picture
                    var approved = Status.Approved.ToString();
                    var profilePicture = await _database.PICTUREs
                        .Where(p => p.STUDENT_NETNAME == student.NETNAME && p.STATUS == approved)
                        .FirstOrDefaultAsync();

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
        public async Task<dynamic> GetAllAsync()
        {

            var students = await _database.STUDENTS
                .Select(s => new
                {
                    s.ID,
                    s.NETNAME,
                    s.FIRSTNAME,
                    s.LASTNAME,
                    s.DOB
                })
                .ToListAsync();

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
                .Where(s => s.ID == pictureValidation.Id)
                .FirstOrDefault();

            var studentNetname = student.NETNAME;


            var pending = Status.Pending.ToString();
            var pendingPicture = _database.PICTUREs.
                Where(p => p.STATUS == pending && p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

            if(pictureValidation.Valid)
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
                 .Where(p => p.ID_PK == pictureValidation.Id)
                 .FirstOrDefault();

            var studentNetname = selectedPicture.STUDENT_NETNAME;

            //find valid profile picture if user has one 
            var approved = Status.Approved.ToString();
            var currentProfilePicture = _database.PICTUREs
                .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == approved)
                .FirstOrDefault();


            if (pictureValidation.Valid)
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

        /// <summary>
        ///  Retrieve the update period for the current academic year
        /// </summary>
        /// <returns></returns>
        public async Task<PicturePeriod> GetUpdatePicturePeriodAsync()
        {
            // May 1st 2016 start of academic year 2016-17 : summer 2016, fall 16, winter 17

            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;


            int academicYear;
            if (month >= 5)
            {
                academicYear = year;
            }
            else
            {
                academicYear = year - 1;
            }

            var period = await _database.PICTUREUPDATESETTINGs
                .AsNoTracking()
                .Where(p => p.YEAR == academicYear)
                .FirstOrDefaultAsync();

            if(period != null)
            {
                var today = DateTime.Today;

                var canUpdate = today >= period.STARDATE && today <= period.ENDDATE;

                var picturePeriod = new PicturePeriod
                {
                    CanUpdatePicture = canUpdate,
                    StartDate = period.STARDATE.ToString(),
                    EndDate = period.ENDDATE.ToString()
                };

                return picturePeriod;
            }
            else
            {
                var picturePeriod = new PicturePeriod
                {
                    CanUpdatePicture = false,
                    StartDate = string.Empty,
                    EndDate = string.Empty
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
            if(string.IsNullOrEmpty(searchOptions.Birthdate) && !searchOptions.Id.HasValue && searchOptions.Name.Capacity == 0 && string.IsNullOrEmpty(searchOptions.Netname))
            {
                return Enumerable.Empty<STUDENT>();
            }

            //else search 
            IQueryable<STUDENT> student = _database.STUDENTS;

            //each if statement will try to build the where clauses and it only be executed when ToList() is called 
            if (searchOptions.Id.HasValue)
            {
                if (StudentHelper.ValidId(searchOptions.Id.Value))
                {
                    student = student.Where(s => s.ID == searchOptions.Id.Value);
                }
            }
            if (searchOptions.Name.Count != 0)
            {
                foreach (var name in searchOptions.Name)
                {
                    student = student.Where(s => s.FIRSTNAME.Contains(name.ToLower()) || s.LASTNAME.Contains(name.ToLower()));
                }
            }
            if (!string.IsNullOrEmpty(searchOptions.Birthdate))
            {
                DateTime dateValue;
                DateTime.TryParse(searchOptions.Birthdate, out dateValue);

                student = student.Where(s => s.DOB == dateValue);
            }
            if (!string.IsNullOrEmpty(searchOptions.Netname))
            {
                student = student.Where(s => s.NETNAME.Contains(searchOptions.Netname));
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
