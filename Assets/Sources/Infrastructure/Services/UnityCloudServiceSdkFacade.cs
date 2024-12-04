using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;

namespace Sources.Infrastructure.Services
{
	public class UnityCloudServiceSdkFacade : ICloudServiceSdkFacade
	{
		private bool _isAuthorized = false;

		public UniTask<IPlayerAccount> GetPlayerAccount() =>
			new(null);

		public void SetStatusInitialized() { }

		public bool IsAuthorized => CheckAuthorization();

		public UniTask<string> GetPlayerLanguage() =>
			new("ru");

		public UniTask Authorize()
		{
			_isAuthorized = true;
			return UniTask.CompletedTask;
		}

		private bool CheckAuthorization() =>
			_isAuthorized;
	}
}