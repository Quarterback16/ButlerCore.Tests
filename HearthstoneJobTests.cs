using ButlerCore.Jobs;

namespace ButlerCore.Tests
{
    [TestClass]
    public class HearthstoneJobTests
    {
        public HearthstoneJobMaster? Cut { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Cut = new HearthstoneJobMaster(
                new NullLogger(),
                "d:\\Dropbox\\",
                "l:\\apps\\dd\\gevents.json");
        }

        [TestMethod]
        public void HJM_CanDoMetaChampionReport()
        {
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(
                    Cut?.DoMetaChampReport()));
        }

        [TestMethod]
        public void HJM_CanDoChampDeckReport()
        {
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(
                    Cut?.DoChampDeckReport()));
        }

        [TestMethod]
        public void HJM_CanDoWinLossGraph()
        {
            var md = Cut?.DoWinLossGraph();
            Assert.IsFalse(string.IsNullOrWhiteSpace(md));
            Console.WriteLine(md);
        }

    }
}
