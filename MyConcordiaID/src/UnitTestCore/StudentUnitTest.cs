using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace UnitTestCore
{
    /// <summary>
    /// Testing with mocking framework https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
    /// </summary>
    [TestClass]
    public class StudentUnitTest
    {

        private Mock<DatabaseEntities> _context;
        private StudentRepository _repo;
        private PictureRepository _picture;
        private Mock<DbSet<STUDENT>> _mySetStudent;
        private Mock<DbSet<PICTURE>> _mySetPicture;

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
        /// <summary>
        /// http://stackoverflow.com/questions/13766198/c-sharp-accessing-property-values-dynamically-by-property-name
        ///  Access proprerty dynamically by property name
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }
        /// <summary>
        ///  For Viewing all available properties of a dynamic object
        /// </summary>
        /// <param name="source"></param>
        private static void PrintPropertiesOfDynamicObject(object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;

                var propetyValue = source.GetType().GetProperty(property.Name).GetValue(source, null);

                Console.Write(propertyName + " : " + propetyValue);
                Console.WriteLine();
            }
        }


        private static byte[] GetImageByte(IMock<IFormFile> image)
        {
            var stream = image.Object.OpenReadStream();
            byte[] byteImage;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byteImage = memoryStream.ToArray();
            }

            return byteImage;
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<DatabaseEntities>();
            _mySetStudent = new Mock<DbSet<STUDENT>>();
            _mySetPicture = new Mock<DbSet<PICTURE>>();
            _repo = new StudentRepository(_context.Object);
            _picture = new PictureRepository(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _mySetPicture = null;
            _mySetStudent = null;
            _repo = null;
        }

        [TestMethod]
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
            Assert.AreEqual(21941097, student.Id);
            Assert.AreEqual("testFirst", student.FirstName);

        }

        [TestMethod]
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

            Assert.AreEqual(2, Enumerable.Count(result));

        }

        [TestMethod]
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


            _picture.AddPendingPicture(netname, GetImageByte(fileMock));

            _mySetPicture.Verify(m => m.Add(It.IsAny<PICTURE>()), Times.Once());
            _context.Verify(m => m.SaveChanges(), Times.Once());
            
        }

        [TestMethod]
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

            Assert.AreEqual(Status.Approved.ToString(), picture.STATUS);

        }

        [TestMethod]
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
            foreach(dynamic d in studentPictures.ArchivedPictures)
            {
                PrintPropertiesOfDynamicObject(d);
                id = ReflectPropertyValue(d, "ID_PK");
                status = ReflectPropertyValue(d, "STATUS");
            }

            Assert.AreEqual(Status.Denied.ToString(), status);
            Assert.AreEqual(pictureId, id);


        }

        [TestMethod]
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

            Assert.AreEqual(result, netName);
            Assert.AreEqual(result, netNameTest2);
            Assert.AreEqual("a_abcdef", bigNetName);
        }
    }
}
