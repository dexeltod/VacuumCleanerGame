using Agava.YandexGames;
using Cysharp.Threading.Tasks;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexSDKController
	{
		UniTask<PlayerAccountProfileDataResponse> GetPlayerAccount();
		void SetStatusInitialized();
	}
}