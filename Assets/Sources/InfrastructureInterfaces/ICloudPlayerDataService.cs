using Cysharp.Threading.Tasks;
using Sources.Infrastructure.StateMachine.GameStates;

namespace Sources.ApplicationServicesInterfaces
{
	public interface ICloudPlayerDataService
	{
		UniTask<IPlayerAccount> GetPlayerAccount();
		void SetStatusInitialized();
		void Authorize();
	}
}