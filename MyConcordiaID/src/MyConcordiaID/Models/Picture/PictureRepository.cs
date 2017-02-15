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

            var aproved = Status.Approved.ToString();
            var pending = Status.Pending.ToString();
            var denied = Status.Denied.ToString();


            if (student != null)
            {
                var myPictures = _database.PICTUREs
                    .Where(s => s.STUDENT_NETNAME == student.NETNAME)
                    .GroupBy(item => item.STATUS)
                    .Select(group => new
                    {
                        Status = group.Key,
                        Items = group.Select(item => new
                        {
                            item.ID_PK,
                            item.PICTURE_DATA,
                            item.STATUS,
                            item.CREATED,
                            item.UPDATED,
                            item.COMMENTS
                        })
                    });

                StudentPictures pictures = new StudentPictures
                {
                    profilePicture = myPictures
                        .Where(item => item.Status == aproved)
                        .Select(list => list.Items.FirstOrDefault())
                        .FirstOrDefault(),

                    pendingPicture = myPictures
                        .Where(item => item.Status == pending)
                        .Select(list => list.Items.FirstOrDefault())
                        .FirstOrDefault(),

                    archivedPictures = myPictures
                        .Where(item => item.Status != aproved && item.Status != pending)
                        .SelectMany(list => list.Items)
                        .OrderByDescending(order => order.CREATED)
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
                STUDENT_NETNAME = netname

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
