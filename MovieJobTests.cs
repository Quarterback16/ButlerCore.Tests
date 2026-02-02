using ButlerCore.Jobs;
using ButlerCore.Models;

namespace ButlerCore.Tests
{
    [TestClass]
    public class MovieJobTests
    {
        public MovieJobMaster? Cut { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Cut = new MovieJobMaster(
                logger: new NullLogger(),
                dropBoxFolder: "d:\\Dropbox\\",
                movieRootFolder: "m:\\");
        }

        [TestMethod]
        public void MovieDetector_InstantiatesOk()
        {
            Assert.IsNotNull(Cut);
        }

        [TestMethod]
        public void MovieDetector_CanDoDetectorJob()
        {
            Assert.IsNotNull(Cut);
            Cut.DoDetectorJob(
                new MovieService.MovieService());
        }

        [TestMethod]
        public void MovieDetector_CanDoCullJob()
        {
            Assert.IsNotNull(Cut);
            Cut.DoCullJob();
        }

        [TestMethod]
        public void MovieDetector_CanDoSingleMovie()
        {
            List<Movie> list = Cut.GetMovieList();
            Assert.IsNotNull(list);
            list = list.Where(l => l.Title == "Rebel Moon 2")
                .ToList();
            foreach (var movie in list)
            {
                if (Cut.IsMarkdownFor(movie.Title))
                {
                    Console.WriteLine(
                        $"{Cut.MarkdownFileName(movie.Title)} already exists");
                    continue;
                }

                Cut.WriteMovieMarkdown(
                    movie,
                    new MovieService.MovieService());
            }
        }

        [TestMethod]
        public void MovieDetector_CanUpdateMovieWithPosterWhereMdDoesNotExist()
        {
            List<Movie> list = Cut.GetMovieList();
            Assert.IsNotNull(list);
            foreach (var movie in list)
            {
                if (Cut.IsMarkdownFor(movie.Title))
                {
                    Console.WriteLine(
                        $"{Cut.MarkdownFileName(movie.Title)} already exists: dont overwrite it");
                    continue;
                }
                Cut.WriteMovieMarkdown(
                    movie,
                    new MovieService.MovieService());
            }
        }

        [TestMethod]
        public void MovieDetector_CanGenerateMovieList()
        {
            var movieList = Cut?.GetMovieList();

            Assert.IsNotNull(movieList);

            movieList.ForEach(
                movie =>
                {
                    Console.WriteLine($"{movie.Title} {movie.Year}");
                });
        }

        [TestMethod]
        public void MovieDetector_KnowsIfThereIsAMarkdownFile()
        {
            var isMarkdown = Cut?.IsMarkdownFor("Akira");

            Assert.IsTrue(isMarkdown);
        }

        [TestMethod]
        public void MovieDetector_CanReadPropertiesInAMovieMarkdownFile()
        {
            var propertyValue = Cut?.MovieProperty(
                "Akira",
                "Keeper");

            Assert.AreEqual(
                "N",
                propertyValue);
        }

        [TestMethod]
        public void MovieDetector_CanAccessAllPropertiesInAMovieMarkdownFile()
        {
            var propertyList = Cut?.ReadProperties(
                "Mean Machine");

            Assert.IsNotNull(propertyList);
            propertyList.ForEach(p => Console.WriteLine(p) );
        }

        [TestMethod]
        public void MovieDetector_CanAccessAllTagsInAMovieMarkdownFile()
        {
            var tagList = Cut?.ReadTags(
                "Mean Machine");

            Assert.IsNotNull(tagList);
            tagList.ForEach(tag => Console.WriteLine(tag));
        }

        [TestMethod]
        public void MovieDetector_KnowsTheKeepers()
        {
            var isKeeper = Cut?.IsKeeper(
                "Monty Python and the Holy Grail");

            Assert.IsTrue(isKeeper);
        }

        [TestMethod]
        public void MovieDetector_KnowsTheKeepersWanda()
        {
            var isKeeper = Cut?.IsKeeper(
                "A Fish called Wanda");

            Assert.IsTrue(isKeeper);
        }

        [TestMethod]
        public void MovieDetector_KnowsTheNonKeepers()
        {
            var isKeeper = Cut?.IsKeeper(
                "Akira");

            Assert.IsFalse(isKeeper);
        }

        [TestMethod]
        public void MovieDetector_KnowsWatchedMovie()
        {
            var haveWatched = Cut?.Watched(
                new Movie(
                    "Mean Machine", 
                    string.Empty,
                    "Mean Machine.mp4"));

            Assert.IsTrue(haveWatched);
        }

        [TestMethod]
        public void MovieDetector_KnowsUnWatchedMovie()
        {
            var haveWatched = Cut?.Watched(
                new Movie(
                    "Wicked For Good", 
                    string.Empty,
                    "Wicked For Good.mp4"));

            Assert.IsFalse(haveWatched);
        }

        [TestMethod]
        public void MovieDetector_KnowsUnprocessedFiles()
        {
            var result = Cut?.UnprocessedFiles();
            Assert.IsNotNull(result);
            result.ForEach(file => Console.WriteLine(file));
        }

        [TestMethod]
        public void MovieDetector_CanGenerateCullList()
        {
            var cullList = Cut?.CullList();

            Assert.IsNotNull(cullList);
            Console.WriteLine($"{cullList.Count} movies to cull");
            Console.WriteLine();
            cullList.ForEach(m => Console.WriteLine(m));
        }

        [TestMethod]
        public void MovieDetector_CanParseOutYearSquareBrackets()
        {
            var fileInfo = new FileInfo("D:\\Movies\\Akira [2020] [1080p].mkv");
            var movie = MovieJobMaster.ParseMovie(fileInfo);

            Console.WriteLine($"{movie.Title} {movie.Year}");

            Assert.IsTrue(movie.Title == "Akira");
            Assert.IsTrue(movie.Year == "2020");
        }

        [TestMethod]
        public void MovieDetector_CanParseOutYearRoundBrackets()
        {
            var fileInfo = new FileInfo("D:\\Movies\\Akira (2020) [1080p].mkv");
            var movie = MovieJobMaster.ParseMovie(fileInfo);

            Console.WriteLine($"{movie.Title} {movie.Year}");

            Assert.IsTrue(movie.Title == "Akira");
            Assert.IsTrue(movie.Year == "2020");
        }

        [TestMethod]
        public void MovieDetector_CanParseOutTitle()
        {
            var fileInfo = new FileInfo("D:\\Movies\\Akira (1988) [1080p].mkv");
            var movie = MovieJobMaster.ParseMovie(fileInfo);

            Console.WriteLine($"{movie.Title} {movie.Year} {movie.FileName}");

            Assert.IsTrue(movie.Title == "Akira");
            Assert.IsTrue(movie.FileName == "Akira (1988) [1080p].mkv");
        }

        [TestMethod]
        public void MovieDetector_CanCreateMarkdownFile()
        {
            var markdown = MovieJobMaster.MovieToMarkdown(
                new Movie 
                {
                    Title = "Akira",
                    Year = "1988",
                    FileName = "Akira (1988) [1080p].mkv"
                },
                new MovieService.MovieService());

            Assert.IsTrue(!string.IsNullOrEmpty(markdown));
            Console.WriteLine(markdown);
        }

        [TestMethod]
        public void MovieDetector_CanSaveMovieFile()
        {
            var success = Cut?.WriteMovieMarkdown(
                new Movie
                {
                    Title = "The Magnificent Seven Ride",
                },
                new MovieService.MovieService());
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void MovieDetector_CanOverwriteSingleMovie()
        {
            List<Movie> list = Cut.GetMovieList();
            Assert.IsNotNull(list);
            list = list.Where(l => l.Title == "The Shawshank Redemption").ToList();
            foreach (var movie in list)
            {
                if (Cut.IsMarkdownFor(movie.Title))
                {
                    Console.WriteLine($"Overwriting {Cut?.MarkdownFileName(movie.Title)}");
                }

                Cut?.WriteMovieMarkdown(
                    movie,
                    new MovieService.MovieService());
            }
        }

        [TestMethod]
        public void MovieDetector_CanGenerateImageLink()
        {
            var link = Cut?.MoviePosterMarkdownLink(
                new Movie
                {
                    Title = "Crouching Tiger Hidden Dragon",
                },
                new MovieService.MovieService());
            Assert.IsFalse(string.IsNullOrEmpty(link));
            Console.WriteLine(link);
        }

        [TestMethod]
        public void MovieDetector_CanGenerateFullPlot()
        {
            var link = Cut?.MoviePlotMarkdown(
                new Movie
                {
                    Title = "Crouching Tiger Hidden Dragon",
                },
                new MovieService.MovieService());
            Assert.IsFalse(string.IsNullOrEmpty(link));
            Console.WriteLine(link);
        }
    }
}
