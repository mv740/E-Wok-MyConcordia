using Microsoft.AspNetCore.Http;
using Moq;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
{
   
    public class StudentRepositoryTest :IDisposable
    {

        private Mock<DatabaseEntities> _context;
        private StudentRepository _repo;
        private PictureRepository _picture;
        private Mock<DbSet<STUDENT>> _mySetStudent;
        private Mock<DbSet<PICTURE>> _mySetPicture;


        public StudentRepositoryTest()
        {
            _context = new Mock<DatabaseEntities>();
            _mySetStudent = new Mock<DbSet<STUDENT>>();
            _mySetPicture = new Mock<DbSet<PICTURE>>();
            _repo = new StudentRepository(_context.Object);
            _picture = new PictureRepository(_context.Object);
        }

        public void Dispose()
        {
            _context = null;
            _mySetPicture = null;
            _mySetStudent = null;
            _repo = null;

        }

        private void ConnectMocksToDataStore(IEnumerable<STUDENT> dataStore)
        {
            var dataSource = dataStore.AsQueryable();
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<STUDENT>(dataSource.Provider));
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.Expression).Returns(dataSource.Expression);
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.ElementType).Returns(dataSource.ElementType);
            _mySetStudent.As<IDbAsyncEnumerable<STUDENT>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<STUDENT>(dataSource.GetEnumerator()));
            _context.Setup(a => a.STUDENTS).Returns(_mySetStudent.Object);
        }

        private void ConnectPictureMocksToDataStore(IEnumerable<PICTURE> dataStore)
        {
            var dataSource = dataStore.AsQueryable();
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<PICTURE>(dataSource.Provider));
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.Expression).Returns(dataSource.Expression);
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.ElementType).Returns(dataSource.ElementType);
            _mySetPicture.As<IDbAsyncEnumerable<PICTURE>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<PICTURE>(dataSource.GetEnumerator()));
            _context.Setup(a => a.PICTUREs).Returns(_mySetPicture.Object);
        }

        
        [Fact]
        public void FindById()
        {
            const string name = "testFirst";
            const string lastName = "testLast";


            var users = new List<STUDENT>()
            {
                new STUDENT
            {
                NETNAME = StudentHelper.GenerateNetName(name, lastName),
                ID = 21941097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            }
            };


            _mySetStudent.Object.AddRange(users);

            ConnectMocksToDataStore(users);


            var student = _repo.FindById(21941097).Result;
            Assert.Equal(21941097, student.Id);
            Assert.Equal("testFirst", student.FirstName);

        }

        [Fact]
        public void GetAll()
        {
            const string name = "testFirst";
            const string lastName = "testLast";


            var users = new List<STUDENT>()
            {
                new STUDENT
            {
                NETNAME = StudentHelper.GenerateNetName(name, lastName),
                ID = 21941097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            },

            new STUDENT
            {
                NETNAME = StudentHelper.GenerateNetName(name, lastName),
                ID = 11141097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            }
        };

            _mySetStudent.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var result = _repo.GetAll().Result;

            Assert.Equal(2, Enumerable.Count(result));

        }

        [Fact]
        public void AddPendingPicture()
        {
            const string name = "testFirst";
            const string lastName = "testLast";

            var netname = StudentHelper.GenerateNetName(name, lastName);
            var netname2 = StudentHelper.GenerateNetName("test", "test");

            var users = new List<STUDENT>()
            {
                new STUDENT
            {
                NETNAME = netname,
                ID = 21941097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U",
                UPDATEPICTURE = false,
                VALID = false
            },

            new STUDENT
            {
                NETNAME = netname2,
                ID = 11141097,
                FIRSTNAME = "test",
                LASTNAME = "test",
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            }
        };

            _mySetStudent.Object.AddRange(users);
            ConnectMocksToDataStore(users);


            var pictures = new List<PICTURE>()
            {
                new PICTURE
                {
                    STATUS = Status.Denied.ToString(),
                    CREATED = DateTime.Now,
                    STUDENT_NETNAME = netname2,
                    PICTURE_DATA = Encoding.ASCII.GetBytes("bytes")
                }
            };



            _mySetPicture.Object.AddRange(pictures);
            ConnectPictureMocksToDataStore(pictures);

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            const string s = "Hello World from a Fake File"; // Testing purpose string byte array 
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(s);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);


            _picture.AddPendingPicture(netname, UnitTestHelper.GetImageByte(fileMock));

            _mySetPicture.Verify(m => m.Add(It.IsAny<PICTURE>()), Times.Once());
            _context.Verify(m => m.SaveChanges(), Times.Once());

        }

        [Fact]
        public void FindPendingPicture()
        {
            const string name = "testFirst";
            const string lastName = "testLast";


            var netname = StudentHelper.GenerateNetName(name, lastName);

            var users = new List<STUDENT>()
            {
                new STUDENT
            {
                NETNAME = netname,
                ID = 21941097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            },
                new STUDENT
            {
                NETNAME = "test",
                ID = 11941097,
                FIRSTNAME = name,
                LASTNAME = lastName,
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            },

        };

            _mySetStudent.Object.AddRange(users);
            ConnectMocksToDataStore(users);



            var pictures = new List<PICTURE>()
            {
                new PICTURE
            {
                PICTURE_DATA = null,
                STUDENT_NETNAME = netname,
                STATUS = Status.Approved.ToString(),
                CREATED = new DateTime(1989,11,06)

            }, new PICTURE
            {
                PICTURE_DATA = null,
                STUDENT_NETNAME = "test",
                STATUS = Status.Approved.ToString(),
                CREATED = new DateTime(1989,11,06)

            },
        };

            _mySetPicture.Object.AddRange(pictures);
            ConnectPictureMocksToDataStore(pictures);


            var picture = _picture.FindPendingPicture(21941097);

            Assert.Equal(Status.Approved.ToString(), picture.STATUS);

        }

        [Fact]
        public void ValidatePicture()
        {
            const string name = "testFirst";
            const string lastName = "testLast";
            var netname = StudentHelper.GenerateNetName(name, lastName);

            var users = new List<STUDENT>()
            {
                new STUDENT
                {
                    NETNAME = netname,
                    ID = 21941097,
                    FIRSTNAME = name,
                    LASTNAME = lastName,
                    DOB = DateTime.UtcNow,
                    UGRADSTATUS = "U"
                },

                 new STUDENT
                {
                    NETNAME = "test",
                    ID = 11941097,
                    FIRSTNAME = name,
                    LASTNAME = lastName,
                    DOB = DateTime.UtcNow,
                    UGRADSTATUS = "U"
                },
            };

            _mySetStudent.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var pictures = new List<PICTURE>()
            {
                new PICTURE
            {
                PICTURE_DATA = null,
                STUDENT_NETNAME = netname,
                STATUS = Status.Pending.ToString(),
                CREATED = new DateTime(1989,11,06)

            }, new PICTURE
            {
                PICTURE_DATA = null,
                STUDENT_NETNAME = "test",
                STATUS = Status.Approved.ToString(),
                CREATED = new DateTime(1989,11,06)

            },
        };

            _mySetPicture.Object.AddRange(pictures);
            ConnectPictureMocksToDataStore(pictures);


            // Admin deny the picture
            var validationMessage = new PictureValidation
            {
                Id = 21941097,
                Valid = false
            };

            var picturePending = _picture.FindPendingPicture(21941097);
            var pictureId = picturePending.ID_PK;

            _repo.ValidatePicture(validationMessage, "test");

            var studentPictures = _picture.FindStudentPictures(21941097).Result;

            decimal id = -1;
            var status = "";
            // in our unit test our student has only 1 picture
            foreach (dynamic d in studentPictures.ArchivedPictures)
            {
                UnitTestHelper.PrintPropertiesOfDynamicObject(d);
                id = UnitTestHelper.ReflectPropertyValue(d, "ID_PK");
                status = UnitTestHelper.ReflectPropertyValue(d, "STATUS");
            }

            Assert.Equal(Status.Denied.ToString(), status);
            Assert.Equal(pictureId, id);


        }

        [Fact]
        public void GenerateStudentNetName()
        {
            const string firstName = "francis";
            const string lastName = "cote-tremblay";

            const string bigFirstName = "abcdefghijklmnop";
            const string bigLastName = "abcdefghijklmnop";

            const string firstNameTest2 = "Francis";
            const string lastNameTest2 = "Côté-Tremblay";



            var netName = StudentHelper.GenerateNetName(firstName, lastName);
            var bigNetName = StudentHelper.GenerateNetName(bigFirstName, bigLastName);
            var netNameTest2 = StudentHelper.GenerateNetName(firstNameTest2, lastNameTest2);

            const string result = "f_cotetr";

            Assert.Equal(result, netName);
            Assert.Equal(result, netNameTest2);
            Assert.Equal("a_abcdef", bigNetName);
        }

    }
}
