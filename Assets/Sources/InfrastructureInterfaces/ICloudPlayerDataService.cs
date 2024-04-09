using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces
{
	public interface ICloudPlayerDataService
	{
		UniTask<IPlayerAccount> GetPlayerAccount();
		void SetStatusInitialized();
		void Authorize();
		bool IsAuthorized { get;}
	}
}