using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;

namespace Sources.Infrastructure.Services
{
	public class UnityCloudServiceSdkFacade : ICloudServiceSdkFacade
	{
		public UniTask<IPlayerAccount> GetPlayerAccount() =>
			new(null);

		public void SetStatusInitialized() { }

		public void Authorize() { }
		public bool IsAuthorized => CheckAuthorization();

		private bool CheckAuthorization()
		{
			return false;
		}
	}
}