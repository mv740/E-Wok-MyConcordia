using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public class PictureRepository : IPictureRepository
    {

        private readonly DatabaseEntities _database;

        public PictureRepository(DatabaseEntities context)
        {
            _database = context;
        }

        public StudentPictures FindStudentPictures(int id)
        {
            var student = _database.STUDENTS
                .Where(s => s.ID == id)
                .FirstOrDefault();

            if(student != null)
            {

                var studentNetname = student.NETNAME;

                //all picture together 
                //var pictures = _database.PICTUREs
                //    .Where(s => s.STUDENT_NETNAME == studentNetname)
                //    .Select(s => new
                //    {
                //        s.ID_PK,
                //        s.PICTURE_DATA,
                //        s.STATUS,
                //        s.CREATED,
                //        s.UPDATED

                //    })
                //    .OrderByDescending(s => s.CREATED)
                //    .ToList();

                var aproved = Status.Approved.ToString();
                var profile = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == aproved)
                    .Select(p => new { p.PICTURE_DATA })
                    .FirstOrDefault();

                var pending = Status.Pending.ToString();
                var pendingPicture = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == pending)
                    .Select(p => new { p.PICTURE_DATA })
                    .FirstOrDefault();

                var archived = Status.Archived.ToString();
                var arhivedPictures = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == aproved)
                    .Select(s => new
                    {
                        s.ID_PK,
                        s.PICTURE_DATA,
                        s.STATUS,
                        s.CREATED,
                        s.UPDATED

                    })
                    .OrderByDescending(s => s.CREATED)
                    .ToList();


                //all pictures by status



                StudentPictures pictures = new StudentPictures
                {
                    profilePicture = (profile == null) ? null : profile.PICTURE_DATA,
                    pendingPicture = (pendingPicture == null) ? null : pendingPicture.PICTURE_DATA,
                    archivedPictures = arhivedPictures
                };

                return pictures;
            }

            return null;// student not found;
        }

        public void AddPendingPicture(string netname, byte[] picture)
        {

            var student = _database.STUDENTS
                .Where(s => s.NETNAME == netname)
                .FirstOrDefault();

            student.UPDATEPICTURE = false; // so he can't send multiple update 
            student.PENDING = true;


            PICTURE pendingPicture = new PICTURE
            {
                PICTURE_DATA = picture,
                STATUS = Status.Pending.ToString(),
                CREATED = DateTime.UtcNow,
                STUDENT_NETNAME = netname,
            };

            _database.PICTUREs.Add(pendingPicture);
            _database.SaveChanges();

        }

        public PICTURE FindPendingPicture(int id)
        {

            var student = _database.STUDENTS
                .Where(s => s.ID == id)
                .Select(s => new { s.NETNAME })
                .FirstOrDefault();

            var pendingPicture = _database.PICTUREs
                .Where(p => p.STUDENT_NETNAME == student.NETNAME)
                .FirstOrDefault();


            return pendingPicture;

        }

    }
}
