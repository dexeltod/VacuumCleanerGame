using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Sources.Controllers
{
    public class UnityServicesController : IUnityServicesController
    {
        private const string TestEnvironmentName = "tests";
        private const string ProductionEnvironmentName = "production";

        private readonly InitializationOptions _options;

        public UnityServicesController(InitializationOptions options) =>
            _options = options;

        public async UniTask InitializeUnityServices()
        {
            _options.SetEnvironmentName(TestEnvironmentName);

            await UnityServices.InitializeAsync(_options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}