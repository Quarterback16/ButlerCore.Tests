using ButlerCore.Helpers;
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
                dropBoxFolder: "d:/Dropbox/",
                bookFolders: new string[] 
                {
                    "b:/IT/",
                    "b:/By Author/"
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
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void ProgramCanRunBookJobs()
        {
            var result = Program.BookJobs(
                new ButlerCoreContext
                {
                    Logger = new NullLogger(),
                    DropBoxFolder = "d:/Dropbox/",
                    HostName = "Elsie",
                    KatlaBookFolders =
                    [
                        "b:/IT/",
                        "b:/By Author/"
                    ],
                    ElsieBookFolders =
                    [
                        "b:/IT/", 
                        "b:/By Author/" 
                    ],
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void BJM_KnowsNewBooksLastNumMonths()
        {
            var results = Cut?.DoDetectorJob(2);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void BJM_KnowsNewBooksByDateRange()
        {
            var results = Cut?.DetectByRange(
                new DateTime(2025,12,1,0,0,0,DateTimeKind.Unspecified),
                new DateTime(2025,12,31,0,0,0,DateTimeKind.Unspecified));
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void MH_KnowsHowToGenerateMarkdownForNewBooks()
        {
            var results = Cut?.DetectByRange(
                new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Unspecified),
                new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Unspecified));
            Assert.IsNotNull(results);
            Console.WriteLine(
                MarkdownHelper.GenerateBookTable(results));
        }
    }
}
