using OracleEntityFramework;
using System;
using System.Linq;

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

            if (student != null)
            {
                var studentNetname = student.NETNAME;

                var aproved = Status.Approved.ToString();
                var profile = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == aproved)
                    .Select(s => new
                    {
                        s.ID_PK,
                        s.PICTURE_DATA,
                        s.STATUS,
                        s.CREATED,
                        s.UPDATED,
                        s.COMMENTS

                    })
                    .FirstOrDefault();

                var pending = Status.Pending.ToString();
                var pendingPicture = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS == pending)
                    .Select(s => new
                    {
                        s.ID_PK,
                        s.PICTURE_DATA,
                        s.STATUS,
                        s.CREATED,
                        s.UPDATED,
                        s.COMMENTS
                    

                    })
                    .FirstOrDefault();

                //all picture that aren't a profile or a pending
                var remainingPicture = _database.PICTUREs
                    .Where(p => p.STUDENT_NETNAME == studentNetname && p.STATUS != aproved && p.STATUS != pending)
                    .Select(s => new
                    {
                        s.ID_PK,
                        s.PICTURE_DATA,
                        s.STATUS,
                        s.CREATED,
                        s.UPDATED,
                        s.COMMENTS

                    })
                    .OrderByDescending(s => s.CREATED)
                    .ToList();


                //all pictures by status



                StudentPictures pictures = new StudentPictures
                {
                    profilePicture = profile,
                    pendingPicture = pendingPicture,
                    archivedPictures = remainingPicture
                };

                return pictures;
            }

            return null;// student not found;
        }

        /// <summary>
        ///  Insert pending picture in database 
        /// </summary>
        /// <param name="netname"></param>
        /// <param name="picture"></param>
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

        public string AddPictureComment(PictureComment comment)
        {
            var picture = _database.PICTUREs
                .Where(p => p.ID_PK == comment.Id)
                .FirstOrDefault();

            if (picture != null)
            {
                picture.COMMENTS = comment.Comment;
                _database.SaveChanges(); // change to async later on
                return picture.STUDENT_NETNAME;

            }
            else
            {
                return string.Empty;
            }
        }
    }
}
