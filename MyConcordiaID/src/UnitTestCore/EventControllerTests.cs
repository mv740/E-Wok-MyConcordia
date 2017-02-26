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
using MyConcordiaID.Models.Event;

namespace UnitTestCore
{
    [TestClass]
    public class EventControllerTests
    {

        private Mock<DatabaseEntities> _context;
        private EventRepository _eventRepo;
        private LogRepository _logs;
        private Mock<DbSet<EVENT_USERS>> _mySetEventUser;
        private Mock<DbSet<EVENT>> _mySetEvent;

        private void ConnectUsersMocksToDataStore(IEnumerable<EVENT_USERS> dataStore)
        {
            var dataSource = dataStore.AsQueryable();
            _mySetEventUser.As<IQueryable<EVENT_USERS>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<EVENT_USERS>(dataSource.Provider));
            _mySetEventUser.As<IQueryable<EVENT_USERS>>().Setup(data => data.Expression).Returns(dataSource.Expression);
            _mySetEventUser.As<IQueryable<EVENT_USERS>>().Setup(data => data.ElementType).Returns(dataSource.ElementType);
            _mySetEventUser.As<IDbAsyncEnumerable<EVENT_USERS>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<EVENT_USERS>(dataSource.GetEnumerator()));
            _context.Setup(a => a.EVENT_USERS).Returns(_mySetEventUser.Object);
        }


        private void ConnectEventMocksToDataStore(IEnumerable<EVENT> dataStore)
        {
            var dataSource = dataStore.AsQueryable();
            _mySetEvent.As<IQueryable<EVENT>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<EVENT>(dataSource.Provider));
            _mySetEvent.As<IQueryable<EVENT>>().Setup(data => data.Expression).Returns(dataSource.Expression);
            _mySetEvent.As<IQueryable<EVENT>>().Setup(data => data.ElementType).Returns(dataSource.ElementType);
            _mySetEvent.As<IDbAsyncEnumerable<EVENT>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<EVENT>(dataSource.GetEnumerator()));
            _context.Setup(a => a.EVENTS).Returns(_mySetEvent.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<DatabaseEntities>();
            _mySetEventUser = new Mock<DbSet<EVENT_USERS>>();
            _mySetEvent = new Mock<DbSet<EVENT>>();
            _eventRepo = new EventRepository(_context.Object);
            _logs = new LogRepository(_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _mySetEventUser = null;
            _mySetEvent = null;
            _logs = null;
        }

        private void SetBasicMockDb()
        {
            //Set Database items
            var today = DateTime.UtcNow;
            var tomorrow = DateTime.UtcNow.AddDays(1);



            var events = new List<EVENT>()
            {
                new EVENT()
                {
                    ID_PK = "1",
                    NAME = "eventName1",
                    DESCRIPTION = "eventDescription1",
                    STATUS = EventStatusType.Scheduled.ToString(),
                    LOCATION = "eventLocation1",
                    ROOM = "eventLocation1",
                    TIME_BEGIN = today,
                    TIME_END = tomorrow,
                    TYPE = EventType.Open.ToString(),
                },

                new EVENT()
                {
                    ID_PK = "2",
                    NAME = "eventName2",
                    DESCRIPTION = "eventDescription2",
                    STATUS = EventStatusType.Cancelled.ToString(),
                    LOCATION = "eventLocation2",
                    ROOM = "eventLocation2",
                    TIME_BEGIN = today,
                    TIME_END = tomorrow,
                    TYPE = EventType.Open.ToString()
                }
            };

            var users = new List<EVENT_USERS>()
            {
                new EVENT_USERS()
                {
                    EVENT_ID = "1",
                    STUDENT_NETNAME_FK = "m_woznia",
                    ROLE = Role.Creator.ToString(),
                    STATUS = UserStatus.EventOrganizer.ToString(),
                    ID_PK = "1"

                }
            };

            _mySetEvent.Object.AddRange(events);
            _mySetEventUser.Object.AddRange(users);
            ConnectUsersMocksToDataStore(users);
            ConnectEventMocksToDataStore(events);
        }

        [TestMethod]
        public void GetEventById()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new EventController(_logs, _eventRepo);

            //Act
            var result = controller.GetById("1") as ObjectResult;
            var myEvent = result.Value as EventInformation;


            //Assert
            Assert.AreEqual("eventName1", myEvent.Name);
            Assert.AreEqual(EventStatusType.Scheduled.ToString(), myEvent.Status);
        
        }

        [TestMethod]
        public void GetEventByIdNotFound()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new EventController(_logs, _eventRepo);

            //Act
            var result = controller.GetById("0"); 

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void CancelEventNotFound()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new EventController(_logs, _eventRepo);

            var cancel = new EventCancelled
            {
                EventId = "0"
            };

            //Act
            var result = controller.CancelEvent(cancel);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void UpdateEventNotFound()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new EventController(_logs, _eventRepo);

            var newEventInfo = new EventInformation()
            {
                EventId = "0",
                Name = "newName",
                Description = "newDescription",
                Location = "newLocation",
                Room = "newRoom"
            };

            //Act
            var result = controller.UpdateEvent(newEventInfo);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }


    }
}
