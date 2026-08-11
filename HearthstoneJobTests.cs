using BattleNetApi.Service;
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
			var result = Cut?.DoMetaChampReport();	
			Assert.IsFalse(
				string.IsNullOrWhiteSpace(
					result));
			Console.WriteLine(result);
		}

		[TestMethod]
		public void HJM_CanDoChampDeckReport()
		{
			var result = Cut?.DoChampDeckReport();
			Assert.IsFalse(
				string.IsNullOrWhiteSpace(
					result));
			Console.WriteLine(result);
		}

		[TestMethod]
		public void HJM_CanDoWinLossGraph()
		{
			var md = Cut?.DoWinLossGraph();
			Assert.IsFalse(string.IsNullOrWhiteSpace(md));
			Console.WriteLine(md);
		}

		[TestMethod]
		public void HJM_KnowsCurrentMeta()
		{
			var hcs = new HearthstoneCardService();
			var currentMeta = hcs.GetCurrentMeta();
			Console.WriteLine(
				$"Current Meta is : {currentMeta}");
			Assert.IsFalse(
				string.IsNullOrWhiteSpace(currentMeta));
		}

	}
}
