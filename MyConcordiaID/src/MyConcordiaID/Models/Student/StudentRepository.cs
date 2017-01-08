﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OracleEntityFramework;
using System.IO;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Helper;
using System.Data.Entity;
using System.Collections;

namespace MyConcordiaID.Models.Student
{
    public class StudentRepository : IStudentRepository
    {

        private readonly DatabaseEntities _database;

        public StudentRepository(DatabaseEntities context)
        {
            _database = context;
        }

        public STUDENT FindById(int id)
        {
            var student = _database.STUDENTS
                 .Where(s => s.ID == id)
                 .SingleOrDefault();

            return student;
        }

        public dynamic FindByNetName(string netName)
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
                     s.PROFILEPICTURE,
                     s.UGRADSTATUS,
                     s.EXPIREDATE,
                     s.UPDATEPICTURE
                 })
                 .SingleOrDefault();


            return student;
        }

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
        /// <param name="pictureValidation"></param>
        /// <returns>student netname</returns>
        public string ValidatePicture(PictureValidation pictureValidation,string netName)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == pictureValidation.id)
                .FirstOrDefault();

            var studentNetname = student.NETNAME;

            var pendingPicture = _database.PICTUREs.
                Where(p => p.STATUS == Status.Pending.ToString() && p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

            if(pictureValidation.valid)
            {
                pendingPicture.STATUS = Status.Approved.ToString();

                //if user has already a profile picture, set to archive
                var profilePicture = _database.PICTUREs.
                Where(p => p.STATUS == Status.Approved.ToString() && p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

                if(profilePicture != null)
                {
                    profilePicture.STATUS = Status.Archived.ToString();
                    profilePicture.UPDATED = DateTime.UtcNow;
                    profilePicture.ADMINISTRATOR = netName;
                }

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
        ///  Approve once more a previous picture 
        /// </summary>
        /// <param name="pictureValidation">id : picture id </param>
        /// <returns>student netname</returns>
        public string RevalidatePicture(PictureValidation pictureValidation, string netName)
        {
            
            var newApprovedPicture = _database.PICTUREs.
                Where(p =>  p.ID_PK == pictureValidation.id)
                .FirstOrDefault();

            var studentNetname = newApprovedPicture.STUDENT_NETNAME;

            //archived picture
            var currentProfilePicture = _database.PICTUREs
                .Where(p => p.STUDENT_NETNAME == studentNetname)
                .FirstOrDefault();

            currentProfilePicture.STATUS = Status.Archived.ToString();
            currentProfilePicture.UPDATED = DateTime.UtcNow;
            currentProfilePicture.ADMINISTRATOR = netName;

            //new profile
            newApprovedPicture.STATUS = Status.Approved.ToString();
            newApprovedPicture.UPDATED = DateTime.UtcNow;
            newApprovedPicture.ADMINISTRATOR = netName;

            _database.SaveChanges();

            return studentNetname;
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
                    startDate = period.STARDATE.ToString("dd-MM-yyyy"),
                    endDate = period.ENDDATE.ToString("dd-MM-yyyy")
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
