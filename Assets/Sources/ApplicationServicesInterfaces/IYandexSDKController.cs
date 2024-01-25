using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexSDKController
	{
		UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount();
		void SetStatusInitialized();
	}

	public interface ICloudSave
	{
		UniTask Save(string json);
		UniTask<string> Load();
		UniTask DeleteSaves(IGameProgressModel gameProgressModel);
	}
}