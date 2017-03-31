using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MyConcordiaID.Controllers;
using MyConcordiaID.Models.Log;
using OracleEntityFramework;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Infrastructure;
using MyConcordiaID.Models.Admin;
using Xunit;

namespace Tests
{
    public class AdminControllerTests : IDisposable
    {

        private Mock<DatabaseEntities> _context;
        private AdminRepository _repo;
        private LogRepository _logs;
        private Mock<DbSet<PICTUREUPDATESETTING>> _myPictureUpdateSettings;


        public AdminControllerTests()
        {
            _context = new Mock<DatabaseEntities>();
            _myPictureUpdateSettings = new Mock<DbSet<PICTUREUPDATESETTING>>();
            _repo = new AdminRepository(_context.Object);
            _logs = new LogRepository(_context.Object);
        }


        public void Dispose()
        {
            _context = null;
            _myPictureUpdateSettings = null;
            _repo = null;
        }

        private void ConnectMocksToDataStore(IEnumerable<PICTUREUPDATESETTING> dataStore)
        {
            var dataSource = dataStore.AsQueryable();
            _myPictureUpdateSettings.As<IQueryable<PICTUREUPDATESETTING>>().Setup(data => data.Provider).Returns(new TestDbAsyncQueryProvider<PICTUREUPDATESETTING>(dataSource.Provider));
            _myPictureUpdateSettings.As<IQueryable<PICTUREUPDATESETTING>>().Setup(data => data.Expression).Returns(dataSource.Expression);
            _myPictureUpdateSettings.As<IQueryable<PICTUREUPDATESETTING>>().Setup(data => data.ElementType).Returns(dataSource.ElementType);
            _myPictureUpdateSettings.As<IDbAsyncEnumerable<PICTUREUPDATESETTING>>().Setup(data => data.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<PICTUREUPDATESETTING>(dataSource.GetEnumerator()));
            _context.Setup(a => a.PICTUREUPDATESETTINGs).Returns(_myPictureUpdateSettings.Object);
        }

        private void SetBasicMockDb()
        {

            var users = new List<PICTUREUPDATESETTING>()
            {
                 new PICTUREUPDATESETTING()
                {
                    STARDATE = DateTime.UtcNow,
                    ENDDATE =  DateTime.UtcNow.AddDays(1),
                    YEAR =  (short) DateTime.Today.AddYears(-1).Year
                },
                new PICTUREUPDATESETTING()
                {
                    STARDATE = DateTime.UtcNow,
                    ENDDATE =  DateTime.UtcNow.AddDays(1),
                    YEAR =  (short) DateTime.Today.Year
                },
                new PICTUREUPDATESETTING()
                {
                    STARDATE = DateTime.UtcNow.AddYears(1),
                    ENDDATE =  DateTime.UtcNow.AddYears(1).AddDays(1),
                    YEAR =  (short) DateTime.Today.AddYears(1).Year
                }
            };

            _myPictureUpdateSettings.Object.AddRange(users);

            ConnectMocksToDataStore(users);
        }

        [Fact]
        public void GetUpdatePicturePeriod()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new AdminController(_repo, _logs);

            //Act
            var result = controller.GetUpdatePicturePeriod() as ObjectResult;
            var period = result.Value as PeriodSetting;


            var year = (short)DateTime.Today.Year;
            var month = DateTime.Today.Month;
            if (month < 5)
            {
                year--;
            }

            //Assert
            Assert.Equal(year, period.Year);


        }

        [Fact]
        public void GetAllUpdatePicturePeriod()
        {
            //Arrange
            SetBasicMockDb();
            var controller = new AdminController(_repo, _logs);

            //Act
            var result = controller.GetAllUpdatePicturePeriod() as ObjectResult;
            var period = result.Value as List<PeriodSetting>;

            //Assert
            Assert.Equal(3, period.Count);

        }
    }
}
