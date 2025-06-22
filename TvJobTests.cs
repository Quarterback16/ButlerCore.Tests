using ButlerCore.Jobs;
using ButlerCore.Models;

namespace ButlerCore.Tests
{
    [TestClass]
    public class TvJobTests
    {
        public TvJobMaster? Cut { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Cut = new TvJobMaster(
                new NullLogger(),
                "d:\\Dropbox\\",
                "t:\\");
        }

        [TestMethod]
        public void TvDetector_InstantiatesOk()
        {
            Assert.IsNotNull(Cut);
        }

        [TestMethod]
        public void TvDetector_CanDoDetectorJob()
        {
            Assert.IsNotNull(Cut);
            Cut.DoDetectorJob();
        }

        [TestMethod]
        public void TvDetector_CanDoCullJob()
        {
            Assert.IsNotNull(Cut);
            Cut.DoCullJob();
        }

        [TestMethod]
        public void TvDetector_CanDoSingleTv()
        {
            List<Tv> list = Cut.GetTvList();
            Assert.IsNotNull(list);
            list = list.Where(l => l.Title == "Borat").ToList();
            foreach (var Tv in list)
            {
                if (Cut.IsMarkdownFor(Tv.Title))
                {
                    Console.WriteLine($"{Cut.MarkdownFileName(Tv.Title)} already exists");
                    continue;
                }

                Cut.WriteTvMarkdown(Tv);
            }
        }

        [TestMethod]
        public void TvDetector_CanGenerateTvList()
        {
            var TvList = Cut?.GetTvList();

            Assert.IsNotNull(TvList);

            TvList.ForEach(
                Tv =>
                {
                    Console.WriteLine($"{Tv.Title} {Tv.Year}");
                });
        }

        [TestMethod]
        public void TvDetector_KnowsIfThereIsAMarkdownFile()
        {
            var isMarkdown = Cut?.IsMarkdownFor("Akira");

            Assert.IsTrue(isMarkdown);
        }

        [TestMethod]
        public void TvDetector_CanReadPropertiesInATvMarkdownFile()
        {
            var propertyValue = Cut?.TvProperty(
                "Akira",
                "Keeper");

            Assert.AreEqual(
                "N",
                propertyValue);
        }

        [TestMethod]
        public void TvDetector_CanAccessAllPropertiesInATvMarkdownFile()
        {
            var propertyList = Cut?.ReadProperties(
                "House of the Dragon");

            Assert.IsNotNull(propertyList);
            propertyList.ForEach(p => Console.WriteLine(p));
        }

        [TestMethod]
        public void TvDetector_CanAccessAllTagsInATvMarkdownFile()
        {
            var tagList = Cut?.ReadTags(
                 "House of the Dragon");

            Assert.IsNotNull(tagList);
            tagList.ForEach(tag => Console.WriteLine(tag));
        }

        [TestMethod]
        public void TvDetector_KnowsTheKeepers()
        {
            var isKeeper = Cut?.IsKeeper(
                "Monty Python and the Holy Grail");

            Assert.IsTrue(isKeeper);
        }

        [TestMethod]
        public void TvDetector_KnowsTheKeepersWanda()
        {
            var isKeeper = Cut?.IsKeeper(
                "A Fish called Wanda");

            Assert.IsTrue(isKeeper);
        }

        [TestMethod]
        public void TvDetector_KnowsTheNonKeepers()
        {
            var isKeeper = Cut?.IsKeeper(
                "Akira");

            Assert.IsFalse(isKeeper);
        }

        [TestMethod]
        public void TvDetector_KnowsWatchedTv()
        {
            var haveWatched = Cut?.Watched(
                new Tv("Mean Machine", string.Empty));

            Assert.IsTrue(haveWatched);
        }

        [TestMethod]
        public void TvDetector_KnowsUnWatchedTv()
        {
            var haveWatched = Cut?.Watched(
                new Tv("The Reckoning", string.Empty));

            Assert.IsFalse(haveWatched);
        }

        [TestMethod]
        public void TvDetector_KnowsUnprocessedFiles()
        {
            var result = Cut?.UnprocessedFiles();
            Assert.IsNotNull(result);
            result.ForEach(file => Console.WriteLine(file));
        }

        [TestMethod]
        public void TvDetector_CanGenerateCullList()
        {
            var cullList = Cut?.CullList();

            Assert.IsNotNull(cullList);
            Console.WriteLine($"{cullList.Count} Tv Shows to cull");
            Console.WriteLine();
            cullList.ForEach(m => Console.WriteLine(m));
        }

        [TestMethod]
        public void TvDetector_CanParseOutYearSquareBrackets()
        {
            var Tv = TvJobMaster.ParseTv("Akira [2020]");

            Console.WriteLine($"{Tv.Title} {Tv.Year}");

            Assert.IsTrue(Tv.Title == "Akira");
            Assert.IsTrue(Tv.Year == "2020");
        }

        [TestMethod]
        public void TvDetector_CanParseOutYearRoundBrackets()
        {
            var Tv = TvJobMaster.ParseTv("Akira (2020)");

            Console.WriteLine($"{Tv.Title} {Tv.Year}");

            Assert.IsTrue(Tv.Title == "Akira");
            Assert.IsTrue(Tv.Year == "2020");
        }

        [TestMethod]
        public void TvDetector_CanParseOutTitle()
        {
            var Tv = TvJobMaster.ParseTv("Bates Motel (2013)");

            Console.WriteLine($"{Tv.Title} {Tv.Year}");

            Assert.IsTrue(Tv.Title == "Bates Motel");
            Assert.IsTrue(Tv.Year == "2013");
        }

        [TestMethod]
        public void TvDetector_CanCreateMarkdownFile()
        {
            var markdown = TvJobMaster.TvToMarkdown(
                new Tv
                {
                    Title = "Akira"
                });

            Assert.IsTrue(!string.IsNullOrEmpty(markdown));
            Console.WriteLine(markdown);
        }

        [TestMethod]
        public void TvDetector_CanSaveTvFile()
        {
            var success = Cut?.WriteTvMarkdown(
                new Tv
                {
                    Title = "The Magnificent Seven Ride",
                });
            Assert.IsTrue(success);
        }
    }
}
