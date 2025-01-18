using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Sources.Controllers
{
	public class UnityServices
	{
		private const string TestEnvironmentName = "tests";
		private const string ProductionEnvironmentName = "production";

		private readonly InitializationOptions _options;

		public UnityServices(InitializationOptions options) =>
			_options = options;

		public async UniTask InitializeUnityServices()
		{
			_options.SetEnvironmentName(TestEnvironmentName);

			await Unity.Services.Core.UnityServices.InitializeAsync(_options);

			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
	}
}
