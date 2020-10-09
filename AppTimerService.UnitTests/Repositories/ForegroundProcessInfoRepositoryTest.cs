using AppTimerService.Repositories;
using AppTimerService.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void ForegroundProcessInfoRepositoryConstructorTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var expectedStorePath = $"{_helper.StorePath}\\ForegroundProcessInfo.xml";

            // assert items were loaded from store
            Assert.IsTrue(foregroundProcessInfoRepository.Items.Count > 0);
            Assert.IsTrue(File.Exists(expectedStorePath));
        }


        [TestMethod]
        public void GetByIdTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var validId = 6452;
            var expectedProcessName = "devenv";
            var expectedProcessDuration = "00:00:08.0255661";

            // get entity with valid id
            var result = foregroundProcessInfoRepository.GetById(validId);

            // assert result is the correct entity from xml store
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProcessName, result.ProcessName);
            Assert.AreEqual(expectedProcessDuration, result.ForegroundDuration);
        }

        [TestMethod]
        public void GetByIdNullTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var invalidId = 777;

            // get entity with invalid id
            var result = foregroundProcessInfoRepository.GetById(invalidId);

            // assert result is null
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddItemTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var foregroundProcessInfoEntity = _helper.CreateMockEntity(777);

            var expectedCount = foregroundProcessInfoRepository.Items.Count + 1;

            // add entity 
            foregroundProcessInfoRepository.AddItem(foregroundProcessInfoEntity);

            // assert repository contains new item 
            Assert.IsTrue(foregroundProcessInfoRepository.Items.Contains(foregroundProcessInfoEntity));
            Assert.AreEqual(expectedCount, foregroundProcessInfoRepository.Items.Count); 
        }

        [TestMethod]
        public void UpdateItemTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var foregroundProcessInfoEntity = _helper.CreateMockEntity(6452);

            var expectedCount = foregroundProcessInfoRepository.Items.Count;

            // assert entity exists in store with given id
            Assert.IsNotNull(foregroundProcessInfoRepository.GetById(6452));

            // update entity
            foregroundProcessInfoRepository.UpdateItem(foregroundProcessInfoEntity);

            // assert existing entity was updated
            Assert.IsTrue(foregroundProcessInfoRepository.Items.Contains(foregroundProcessInfoEntity));
            Assert.AreEqual(expectedCount, foregroundProcessInfoRepository.Items.Count);
        }

        [TestMethod]
        public void SaveChangesTest()
        {
            var foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);
            var mockNewEntity = _helper.CreateMockEntity(777);
            var mockUpdatedEntity = _helper.CreateMockEntity(6452);

            // assert entity exists in store with given id
            Assert.IsNotNull(foregroundProcessInfoRepository.GetById(6452));

            // add entity
            foregroundProcessInfoRepository.AddItem(mockNewEntity);

            // update entity 
            foregroundProcessInfoRepository.UpdateItem(mockUpdatedEntity);

            // save repository changes
            foregroundProcessInfoRepository.SaveChanges();

            // initialize new repository with the same store path. (initialize Items from the xml store)
            foregroundProcessInfoRepository = new ForegroundProcessInfoRepository(_helper.StorePath);

            // verify new repository contains the added and updated entity
            Assert.IsTrue(_helper.ListContains(foregroundProcessInfoRepository.Items, mockNewEntity));
            Assert.IsTrue(_helper.ListContains(foregroundProcessInfoRepository.Items, mockUpdatedEntity));
        }
    }
}
