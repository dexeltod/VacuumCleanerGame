using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Sources.Application.UnityApplicationServices
{
	public class UnityServicesController
	{
		private const string TestEnvironmentName = "tests";
		private const string ProductionEnvironmentName = "production";

		private readonly InitializationOptions _options;

		public UnityServicesController(InitializationOptions options)
		{
			_options = options;
		}

		public async UniTask InitializeUnityServices()
		{
			InitializationOptions a = _options.SetEnvironmentName(TestEnvironmentName);

			await UnityServices.InitializeAsync(_options);
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
	}
}