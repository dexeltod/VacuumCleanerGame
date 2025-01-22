using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.DomainServices.YandexLeaderboard
{
	public class YandexLeaderboard : IAbstractLeaderBoard
	{
		private const string BoardName = "Points";

		public async UniTask<Dictionary<string, int>> GetPlayers(int playersCount)
		{
			var isResponseReceived = false;
			LeaderboardGetEntriesResponse leaderboardResponse = null;

			Agava.YandexGames.Leaderboard.GetEntries(
				BoardName,
				response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				},
				errorResponse =>
				{
					isResponseReceived = true;
					throw new NullReferenceException("Leaderboard does not loaded" + errorResponse);
				},
				playersCount
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);

			LeaderboardEntryResponse[] entries = leaderboardResponse.entries;

			return entries.ToDictionary(
				response => response.player.publicName,
				response => response.score
			);
		}

		public async UniTask AddScore(int newScore)
		{
			var isResponseReceived = false;
			LeaderboardEntryResponse player = await GetPlayerEntry();

			Agava.YandexGames.Leaderboard.SetScore(BoardName, player.score + newScore, () => isResponseReceived = true);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}

		public async UniTask Set(int score)
		{
			var isResponseReceived = false;

			Agava.YandexGames.Leaderboard.SetScore(BoardName, score, () => isResponseReceived = false);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}

		public async UniTask<Tuple<string, int>> GetPlayer()
		{
			var isResponseReceived = false;
			LeaderboardEntryResponse leaderboardResponse = null;

			Agava.YandexGames.Leaderboard.GetPlayerEntry(
				BoardName,
				response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				},
				ErrorCallback
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);

			return new Tuple<string, int>(leaderboardResponse.player.publicName, leaderboardResponse.score);

			async void ErrorCallback(string errorResponse)
			{
				Agava.YandexGames.Leaderboard.SetScore(BoardName, 0, () => isResponseReceived = true);

				await UniTask.WaitWhile(() => isResponseReceived == false);
			}
		}

		private async UniTask<LeaderboardEntryResponse> GetPlayerEntry()
		{
			var isResponseReceived = false;
			LeaderboardEntryResponse leaderboardResponse = null;

			Agava.YandexGames.Leaderboard.GetPlayerEntry(
				BoardName,
				response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				},
				async errorResponse =>
				{
					Debug.LogError("ERROR IN GETTING PLAYER" + errorResponse);
					Agava.YandexGames.Leaderboard.SetScore(BoardName, 0, () => isResponseReceived = true);

					await UniTask.WaitWhile(() => isResponseReceived == false);
				}
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);

			return leaderboardResponse;
		}
	}
}