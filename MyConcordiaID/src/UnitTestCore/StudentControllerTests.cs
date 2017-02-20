using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MyConcordiaID.Models.Student;
using MyConcordiaID.Controllers;
using MyConcordiaID.Models.Log;
using OracleEntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using MyConcordiaID.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyConcordiaID.Models.Picture;
using System.Data.Entity.Infrastructure;

namespace UnitTestCore
{
    [TestClass]
    public class StudentControllerTests
    {

        private Mock<DatabaseEntities> _context;
        private StudentRepository _repo;
        private PictureRepository _pictures;
        private LogRepository _logs;
        private Mock<DbSet<STUDENT>> _mySetStudent;
        private Mock<DbSet<PICTURE>> _mySetPicture;

        private void ConnectMocksToDataStore(IEnumerable<STUDENT> data_store)
        {
            var data_source = data_store.AsQueryable();
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<STUDENT>(data_source.Provider));
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.Expression).Returns(data_source.Expression);
            _mySetStudent.As<IQueryable<STUDENT>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            _mySetStudent.As<IDbAsyncEnumerable<STUDENT>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<STUDENT>(data_source.GetEnumerator()));
            _context.Setup(a => a.STUDENTS).Returns(_mySetStudent.Object);
        }


        private void ConnectPictureMocksToDataStore(IEnumerable<PICTURE> data_store)
        {
            var data_source = data_store.AsQueryable();
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<PICTURE>(data_source.Provider));
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.Expression).Returns(data_source.Expression);
            _mySetPicture.As<IQueryable<PICTURE>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            _mySetPicture.As<IDbAsyncEnumerable<PICTURE>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<PICTURE>(data_source.GetEnumerator()));
            _context.Setup(a => a.PICTUREs).Returns(_mySetPicture.Object);
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
        /// <summary>
        ///  For Viewing all available properties of a dynamic object
        /// </summary>
        /// <param name="source"></param>
        public static void PrintPropertiesOfDynamicObject(object source)
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

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<DatabaseEntities>();
            _mySetStudent = new Mock<DbSet<STUDENT>>();
            _mySetPicture = new Mock<DbSet<PICTURE>>();
            _repo = new StudentRepository(_context.Object);
            _logs = new LogRepository(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _mySetPicture = null;
            _mySetStudent = null;
            _repo = null;
        }

        private void SetBasicMockDb()
        {
            //Set Database items
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
            },
                 new STUDENT
            {
                NETNAME = StudentHelper.GenerateNetName("michal", "wozniak"),
                ID = 12345678,
                FIRSTNAME = "michal",
                LASTNAME = "wozniak",
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            }
        };


            var pictures = new List<PICTURE>();

            _mySetStudent.Object.AddRange(users);
            _mySetPicture.Object.AddRange(pictures);
            ConnectMocksToDataStore(users);
            ConnectPictureMocksToDataStore(pictures);
        }

        [TestMethod]
        public void GetStudentById()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new StudentController(_repo,_pictures, _logs);

            //Act
            var result = controller.GetById(21941097) as ObjectResult;
            var student = result.Value as StudentAccount;

            var resultStatus = controller.GetById(21941097) as StatusCodeResult;

            //Assert
            Assert.AreEqual(200, StatusCodes.Status200OK);
            Assert.AreEqual(21941097, student.Id);
            Assert.AreEqual("testFirst", student.FirstName);            
        }

        [TestMethod]
        public void GetStudentByIdNotFound()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new StudentController(_repo,_pictures, _logs);

            //Act
            var result = controller.GetById(21111111) as StatusCodeResult;

            //Assert
            Assert.AreEqual(404, StatusCodes.Status404NotFound);

        }

        [TestMethod]
        public void GetAccountFound()
        {
            //Arrange
            SetBasicMockDb();
           
            var controller = new StudentController(_repo, _pictures, _logs);
            IdentityHelper.SetUser("michal", "wozniak", controller);

            //Act
            //var result = controller.GetAccount() as ObjectResult;

            var result = controller.GetAccount() as ObjectResult;

            var account = result.Value as StudentAccount;

            Console.WriteLine(result.Value);

            //assert
            Assert.AreEqual("michal", account.FirstName);
            Assert.AreEqual("wozniak", account.LastName);
        }

    }
}
