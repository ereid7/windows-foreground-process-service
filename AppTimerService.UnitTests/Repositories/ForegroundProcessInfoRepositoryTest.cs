using AppTimerService.Models;
using AppTimerService.Repositories;
using AppTimerService.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace AppTimerService.UnitTests.Repositories
{
    [TestClass]
    public class ForegroundProcessInfoRepositoryTest
    {
        private ForegroundProcessInfoRepositoryHelper _helper;

        public ForegroundProcessInfoRepositoryTest()
        {
            _helper = new ForegroundProcessInfoRepositoryHelper();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _helper.Initialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _helper.Cleanup();
        }

        // TODO create invalid constructor test
        [TestMethod]
        public void ForegroundProcessInfoRepositoryConstructorTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var expectedStorePath = $"{_helper.StorePath}\\ForegroundProcessInfo.xml";

            Assert.IsTrue(foregroundProcessInfoRepository.Items.Count > 0);
            Assert.IsTrue(File.Exists(expectedStorePath));
        }


        //3212

        [TestMethod]
        public void GetByIdTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var validId = 6452;
            var expectedProcessName = "devenv";
            var expectedProcessDuration = "00:00:08.0255661";

            var result = foregroundProcessInfoRepository.GetById(validId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProcessName, result.ProcessName);
            Assert.AreEqual(expectedProcessDuration, result.ForegroundDuration);
        }

        [TestMethod]
        public void GetByIdNullTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var invalidId = 777;

            var result = foregroundProcessInfoRepository.GetById(invalidId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddItemTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var foregroundProcessInfoEntity = new ForegroundProcessInfoEntity();
            var expectedCount = foregroundProcessInfoRepository.Items.Count + 1;

            foregroundProcessInfoEntity.Id = 777;
            foregroundProcessInfoEntity.ProcessName = "mockProcessName";
            foregroundProcessInfoEntity.ForegroundDuration = "00:00:77:777";

            foregroundProcessInfoRepository.AddItem(foregroundProcessInfoEntity);

            Assert.IsTrue(foregroundProcessInfoRepository.Items.Contains((foregroundProcessInfoEntity)));
            Assert.AreEqual(expectedCount, foregroundProcessInfoRepository.Items.Count);
        }

        //[TestMethod]
        //public void UpdateItemTest()
        //{
        //}
    }
}
