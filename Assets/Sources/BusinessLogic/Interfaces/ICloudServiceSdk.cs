using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ICloudServiceSdk
	{
		bool IsAuthorized { get; }
		UniTask Authorize();
		UniTask<IPlayerAccount> GetPlayerAccount();
		UniTask<string> GetPlayerLanguage();
		void SetStatusInitialized();
	}
}