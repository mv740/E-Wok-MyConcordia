using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Threading.Tasks;
using MyConcordiaID.Models.Student;
using MyConcordiaID.Controllers;
using MyConcordiaID.Models.Log;
using OracleEntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using MyConcordiaID.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace UnitTestCore
{
    [TestClass]
    public class StudentControllerTests
    {

        private Mock<DatabaseEntities> _context;
        private StudentRepository _repo;
        private LogRepository _logs;
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
        /// <summary>
        ///  For Viewing all available properties of a dynamic object
        /// </summary>
        /// <param name="source"></param>
        public static void PrintPropertiesOfDynamicObject(object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                var PropertyName = property.Name;

                var PropetyValue = source.GetType().GetProperty(property.Name).GetValue(source, null);

                Console.Write(PropertyName + " : " + PropetyValue);
                Console.WriteLine();
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<DatabaseEntities>();
            _mySet = new Mock<DbSet<STUDENT>>();
            _repo = new StudentRepository(_context.Object);
            _logs = new LogRepository(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _mySet = null;
            _repo = null;
        }

        public void SetBasicMockDb()
        {
            //Set Database items
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
            },
                 new STUDENT
            {
                NETNAME = StudentHelper.GenerateNetName("Michal", "Mozniak"),
                ID = 12345678,
                FIRSTNAME = "Michal",
                LASTNAME = "Wozniak",
                DOB = DateTime.UtcNow,
                UGRADSTATUS = "U"
            }
        };

            _mySet.Object.AddRange(users);
            ConnectMocksToDataStore(users);
        }

        [TestMethod]
        public void GetStudentById()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new StudentController(_repo, _logs);

            //Act
            var result = controller.GetById(21941097) as ObjectResult;
            var student = result.Value as STUDENT;

            //Assert

            Assert.AreEqual(21941097, student.ID);
            Assert.AreEqual("testFirst", student.FIRSTNAME);

        }
        [TestMethod]
        public void GetStudentByIdNotFound()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new StudentController(_repo, _logs);

            //Act
            var result = controller.GetById(21111111);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult)); //404 status

        }

        [TestMethod]
        public void GetAccountNotFound()
        {
            //Arrange
            SetBasicMockDb();
           
            var controller = new StudentController(_repo, _logs);
            IdentityHelper.SetUser("Michal", "Wozniak", controller);


            //Act
            var result = controller.GetAccount() as ObjectResult;

            PrintPropertiesOfDynamicObject(result.Value);

            //assert
            Assert.AreEqual("Michal", ReflectPropertyValue(result.Value,"FIRSTNAME"));
            Assert.AreEqual("Wozniak", ReflectPropertyValue(result.Value, "LASTNAME"));


        }

    }
}
