﻿using System;
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

        public STUDENT FindById(int id)
        {
            var student = _database.STUDENTS
                 .Where(s => s.ID == id)
                 .SingleOrDefault();

            return student;
        }

        public STUDENT FindByNetName(string netName)
        {
            var student = _database.STUDENTS
                 .Where(s => s.NETNAME == netName)
                 .SingleOrDefault();

            return student;
        }

        public dynamic GetAll()
        {

            var students = _database.STUDENTS.Select(s => new { s.ID, s.NETNAME, s.FIRSTNAME, s.LASTNAME });

            return students;
        }

        public void AddPendingPicture(string netname, byte[] picture)
        {

            var student = _database.STUDENTS
                .Where(s => s.NETNAME == netname)
                .FirstOrDefault();

            student.PENDINGPICTURE = picture;
            student.PENDING = true;
            student.UPDATEPICTURE = false; // so he can't send multiple update 

            _database.SaveChanges();

        }

        public dynamic FindPendingPicture(int id)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == id)
                .Select(s => new { s.ID, s.PENDINGPICTURE })
                .FirstOrDefault();

            return student;

        }

        public void ValidatePicture(PictureValidation pictureValidation)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == pictureValidation.id)
                .FirstOrDefault();

            var netName = student.NETNAME;

            var pictureArchive = _database.PICTUREARCHIVEs;

            if (pictureValidation.valid)
            {



                if (student.VALID)
                {
                    //there is a existing valid profile pic 

                    PICTUREARCHIVE archivingProfile = new PICTUREARCHIVE
                    {
                        NETNAME = netName,
                        STATUS = PictureHelper.GetArchivedStatus(),
                        TIMESTAMP = DateTime.Now,
                        PICTURE = student.PROFILEPICTURE,
                        STUDENT = student

                    };

                    pictureArchive.Add(archivingProfile);
                }



                byte[] validPicture = student.PENDINGPICTURE;
                student.PROFILEPICTURE = validPicture;
                student.PENDINGPICTURE = null;
                student.VALID = true;

            }
            else
            {
                PICTUREARCHIVE archivingDeniedPicture = new PICTUREARCHIVE
                {
                    NETNAME = netName,
                    STATUS = PictureHelper.GetDeniedStatus(),
                    TIMESTAMP = DateTime.Now,
                    PICTURE = student.PENDINGPICTURE,
                    STUDENT = student

                };
                pictureArchive.Add(archivingDeniedPicture);

                student.PENDINGPICTURE = null;
            }
            student.PENDING = false;
            _database.SaveChanges();

        }

        public dynamic GetAllPending()
        {

            var students = _database.STUDENTS
                 .Where(s => s.PENDING == true)
                 .Select(s => new { s.ID, s.NETNAME, s.FIRSTNAME, s.LASTNAME })
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
            _database.SaveChangesAsync();
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

        public List<STUDENT> Search(SearchOptions searchOptions)
        {

            List<string> nameLists = searchOptions.name;




            IQueryable<STUDENT> student = _database.STUDENTS;

            //each if statement will try to build the where clauses and it only be executed when ToList() is called 
            if (searchOptions.id.HasValue)
            {
                if (StudentHelper.ValidId(searchOptions.id.Value))
                {
                    student = student.Where(s => s.ID == searchOptions.id.Value);
                }
            }
            if(nameLists.Count != 0)
            {
                foreach (var name in nameLists)
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
