using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces
{
	public interface ICloudServiceSdkFacade
	{
		UniTask<IPlayerAccount> GetPlayerAccount();
		void SetStatusInitialized();
		UniTask Authorize();
		bool IsAuthorized { get; }
		UniTask<string> GetPlayerLanguage();
	}
}