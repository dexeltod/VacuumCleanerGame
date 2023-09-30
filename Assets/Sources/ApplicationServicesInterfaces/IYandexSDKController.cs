using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.DomainInterfaces;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexSDKController : IService
	{
		UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount();

		UniTask         Save(string json);
		UniTask<string> Load();
		UniTask         DeleteSaves(IGameProgressModel gameProgressModel);
	}
}