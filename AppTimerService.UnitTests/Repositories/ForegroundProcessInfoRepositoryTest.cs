using AppTimerService.Repositories;
using AppTimerService.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }
    }
}
