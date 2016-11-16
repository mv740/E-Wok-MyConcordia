using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Picture;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace UnitTestCore
{
    [TestClass]
    public class StudentUnitTest
    {

        private Mock<DatabaseEntities> _context;
        private StudentRepository _repo;
        private Mock<DbSet<STUDENT>> _mySet;

        private void ConnectMocksToDataStore(IEnumerable<STUDENT> data_store)
        {
            var data_source = data_store.AsQueryable();
            _mySet.As<IQueryable<STUDENT>>().Setup(data => data.Provider).Returns(data_source.Provider);
            _mySet.As<IQueryable<STUDENT>>().Setup(data => data.Expression).Returns(data_source.Expression);
            _mySet.As<IQueryable<STUDENT>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            _mySet.As<IQueryable<STUDENT>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());
            _context.Setup(a => a.STUDENTS).Returns(_mySet.Object);
        }

        /// <summary>
        /// http://stackoverflow.com/questions/13766198/c-sharp-accessing-property-values-dynamically-by-property-name
        ///  Access proprerty dynamically by property name
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<DatabaseEntities>();
            _mySet = new Mock<DbSet<STUDENT>>();
            _repo = new StudentRepository(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _mySet = null;
            _repo = null;
        }

        [TestMethod]
        public void FindById()
        {
            var name = "testFirst";
            var lastName = "testLast";


            List<STUDENT> users = new List<STUDENT>()
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

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var student = _repo.Find(21941097);
            Assert.AreEqual(21941097, student.ID);
            Assert.AreEqual("testFirst", student.FIRSTNAME);

        }

        [TestMethod]
        public void GetAll()
        {
            var name = "testFirst";
            var lastName = "testLast";


            List<STUDENT> users = new List<STUDENT>()
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

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var result = _repo.GetAll();

            Assert.AreEqual(2, Enumerable.Count(result));

        }

        [TestMethod]
        public void AddPendingPicture()
        {
            var name = "testFirst";
            var lastName = "testLast";


            List<STUDENT> users = new List<STUDENT>()
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
        };

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var s = "Hello World from a Fake File"; // Testing purpose string byte array 
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(s);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);

            _repo.AddPendingPicture(21941097, fileMock.Object);


            var student = _repo.Find(21941097);

            Assert.AreEqual(true, student.PENDING);
            Assert.IsFalse(student.PENDINGPICTURE == null);  // not null

        }

        [TestMethod]
        public void FindPendingPicture()
        {
            var name = "testFirst";
            var lastName = "testLast";


            List<STUDENT> users = new List<STUDENT>()
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
        };

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var s = "Hello World from a Fake File"; // Testing purpose string byte array 
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(s);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);

            _repo.AddPendingPicture(21941097, fileMock.Object);


            var student = _repo.FindPendingPicture(21941097);


            long fileLength = fileMock.Object.Length;


            var PENDINGPICTURE = ReflectPropertyValue(student, "PENDINGPICTURE");
            var ID = ReflectPropertyValue(student, "ID");


            Assert.AreEqual(fileLength, PENDINGPICTURE.Length);
            Assert.AreEqual(21941097, ID);


        }

        [TestMethod]
        public void ValidatePicture()
        {
            var name = "testFirst";
            var lastName = "testLast";


            List<STUDENT> users = new List<STUDENT>()
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
            };

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var s = "Hello World from a Fake File"; // Testing purpose string byte array 
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(s);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);

            _repo.AddPendingPicture(21941097, fileMock.Object);

            //verify pending picture is actually there
            var student = _repo.Find(21941097);
            Assert.AreEqual(true, student.PENDING);
            Assert.IsFalse(student.PENDINGPICTURE == null);  // not null

            // Admin accept the picture
            PictureValidation validationMessage = new PictureValidation
            {
                id = 21941097,
                valid = true
            };

            _repo.ValidatePicture(validationMessage);

            var studentLatestInfo = _repo.Find(21941097);

            Assert.AreEqual(false, studentLatestInfo.PENDING);
            Assert.AreEqual(null, studentLatestInfo.PENDINGPICTURE);
            Assert.AreEqual(true, studentLatestInfo.VALID);
            Assert.IsFalse(studentLatestInfo.PROFILEPICTURE == null); // there is a picture thus false
        }

        [TestMethod]
        public void GenerateStudentNetName()
        {
            var firstName = "francis";
            var lastName = "cote-tremblay";
      
            var netName = StudentHelper.GenerateNetName(firstName, lastName);
           
          
            var result = "f_cotetr";

            Assert.AreEqual(result, netName);
          
        }

    }




}
