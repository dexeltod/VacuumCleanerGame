using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ICloudServiceSdk
	{
		UniTask<IPlayerAccount> GetPlayerAccount();
		void SetStatusInitialized();
		UniTask Authorize();
		bool IsAuthorized { get; }
		UniTask<string> GetPlayerLanguage();
	}
}
