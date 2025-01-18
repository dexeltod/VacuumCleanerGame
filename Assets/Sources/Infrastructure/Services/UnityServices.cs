using Cysharp.Threading.Tasks;
using Unity.Services.Core;

namespace Sources.Infrastructure.Services
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