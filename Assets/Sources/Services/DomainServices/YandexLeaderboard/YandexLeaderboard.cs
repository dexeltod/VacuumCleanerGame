using System.Collections.Generic;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Services.DomainServices.YandexLeaderboard
{
	public class YandexLeaderboard : ILeaderBoard
	{
		private const string BoardName = "Points";

		public async UniTask<Dictionary<string, int>> GetPlayers(int playersCount)
		{
			Dictionary<string, int> playersLeaders = new Dictionary<string, int>();

			bool isResponseReceived = false;
			LeaderboardGetEntriesResponse leaderboardResponse = null;

			Leaderboard.GetEntries
			(
				BoardName,
				onSuccessCallback: response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				}
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);

			LeaderboardEntryResponse[] entries = leaderboardResponse.entries;

			foreach (LeaderboardEntryResponse response in entries)
				playersLeaders.Add(response.player.publicName, response.score);

			return playersLeaders;
		}

		public async UniTask Set(int score)
		{
			bool isResponseReceived = false;

			Leaderboard.SetScore(BoardName, score, () => isResponseReceived = false);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}

		public async UniTask GetPlayerEntry()
		{
			bool isResponseReceived = false;
			LeaderboardEntryResponse leaderboardResponse = null;

			Leaderboard.GetPlayerEntry(
				BoardName,
				response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				}
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}
	}
}