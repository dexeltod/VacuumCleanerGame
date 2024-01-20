using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexSDKController
	{
		UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount();
		UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback);

		UniTask Save(string json);
		UniTask<string> Load();
		UniTask DeleteSaves(IGameProgressModel gameProgressModel);
		void SetStatusInitialized();
	}
}