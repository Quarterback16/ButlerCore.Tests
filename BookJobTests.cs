using ButlerCore.Jobs;

namespace ButlerCore.Tests
{
    [TestClass]
    public class BookJobTests
    {
        public BookJobMaster? Cut { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Cut = new BookJobMaster(
                logger: new NullLogger(),
                bookFolders: new string[] 
                {
                    "b:\\IT\\"
                });
        }

        [TestMethod]
        public void BJM_InstantiatesOk()
        {
            Assert.IsNotNull(Cut);
        }

        [TestMethod]
        public void BJM_KnowsNewBooksThisMonth()
        {
            var results = Cut?.DoDetectorJob(0);
            Assert.AreEqual(0, results);
        }

        [TestMethod]
        public void BJM_KnowsNewBooksLastNumMonths()
        {
            var results = Cut?.DoDetectorJob(2);
            Assert.AreEqual(0, results);
        }
    }
}
