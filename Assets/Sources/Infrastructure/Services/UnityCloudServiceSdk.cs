using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Interfaces;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Services
{
	public class UnityCloudServiceSdk : ICloudServiceSdk
	{
		private bool _isAuthorized;

		public UniTask<IPlayerAccount> GetPlayerAccount() => new(null);

		public void SetStatusInitialized()
		{
		}

		public bool IsAuthorized => CheckAuthorization();

		public UniTask<string> GetPlayerLanguage() => new("ru");

		public UniTask Authorize()
		{
			_isAuthorized = true;
			return UniTask.CompletedTask;
		}

		private bool CheckAuthorization() => _isAuthorized;
	}
}