namespace ButlerCore.Tests
{
    [TestClass]
    public class TipItJobTests
    {
        [TestMethod]
        public void ProgramCanRunTippingJobs()
        {
            var result = Program.TipitJob(
                new ButlerCoreContext
                {
                    TippingSeason = DateTime.Now.Year,
                    Logger = new NullLogger(),
                    DropBoxFolder = "d:/Dropbox/",
                    ResultsFile = "d:/Dropbox/json/results.json",
                    StartDateTime = DateTime.Now,
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CanCalculateNewTippingState()
        {
            var ts = new TipItService.TipItService(
                "d://dropbox//");

            var newState = ts.GetNewTippingState(
                new DateTime(2026,3,21,0,0,0,DateTimeKind.Unspecified));
            Assert.IsNotNull(newState);
            Console.WriteLine( $"{ts.NewResults.Count} new results added");
            ts.NewResults.ForEach(
                nr =>
                {
                    Console.WriteLine(nr.ToString());
                });
        }

        [TestMethod]
        public void CanCalculateNewTippingStateOverADateRange()
        {
            // Might have missed some dates 
            var startDate = DateTime.Now.AddDays(-10);

            var ts = new TipItService.TipItService(
                "d://dropbox//");

            for (int i = 0; i < 10; i++)
            {
                var testDate = startDate.AddDays(i);
                var newState = ts.GetNewTippingState(testDate);
                Assert.IsNotNull(newState);
                Console.WriteLine(
                    $"{ts.NewResults.Count} new results found on {testDate.ToString("yyyy-MM-dd")}");
                ts.NewResults.ForEach(
                    nr =>
                    {
                        Console.WriteLine(nr.ToString());
                    });
            }
        }

        [TestMethod]
        public void CanDoEasiest()
        {
            var ts = new TipItService.TipItService(
                "d://dropbox//");
            var md = ts.Easiest();
            Assert.IsNotNull(md);
            Console.WriteLine(md);
        }

    }
}
