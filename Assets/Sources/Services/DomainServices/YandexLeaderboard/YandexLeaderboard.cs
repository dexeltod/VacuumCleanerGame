using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices.YandexLeaderboard
{
	public class YandexLeaderboard : IAbstractLeaderBoard
	{
		private const string BoardName = "Points";

		public async UniTask<Dictionary<string, int>> GetPlayers(int playersCount)
		{
			bool isResponseReceived = false;
			LeaderboardGetEntriesResponse leaderboardResponse = null;

			Leaderboard.GetEntries(
				BoardName,
				onSuccessCallback: response =>
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
				response => response.player.scopePermissions.public_name,
				response => response.score
			);
		}

		public async UniTask AddScore(int newScore)
		{
			bool isResponseReceived = false;

			LeaderboardEntryResponse player = await GetPlayerEntry();

			Leaderboard.SetScore(BoardName, player.score + newScore, () => isResponseReceived = true);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}

		public async UniTask Set(int score)
		{
			bool isResponseReceived = false;

			Leaderboard.SetScore(BoardName, score, () => isResponseReceived = false);

			await UniTask.WaitWhile(() => isResponseReceived == false);
		}

		public async UniTask<Tuple<string, int>> GetPlayer()
		{
			bool isResponseReceived = false;
			LeaderboardEntryResponse leaderboardResponse = null;

			Leaderboard.GetPlayerEntry(
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
				Leaderboard.SetScore(BoardName, 0, () => isResponseReceived = true);

				await UniTask.WaitWhile(() => isResponseReceived == false);
			}
		}

		private async UniTask<LeaderboardEntryResponse> GetPlayerEntry()
		{
			bool isResponseReceived = false;
			LeaderboardEntryResponse leaderboardResponse = null;

			Leaderboard.GetPlayerEntry(
				BoardName,
				response =>
				{
					leaderboardResponse = response;
					isResponseReceived = true;
				},
				onErrorCallback: async errorResponse =>
				{
					Debug.LogError("ERROR IN GETTING PLAYER" + errorResponse);
					Leaderboard.SetScore(BoardName, 0, () => isResponseReceived = true);

					await UniTask.WaitWhile(() => isResponseReceived == false);
				}
			);

			await UniTask.WaitWhile(() => isResponseReceived == false);

			return leaderboardResponse;
		}
	}
}