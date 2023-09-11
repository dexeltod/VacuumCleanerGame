using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexSDKController : IService
	{
		UniTask Initialize();
		UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount();

		void Save(string json);
		UniTask<string> Load();
	}
}