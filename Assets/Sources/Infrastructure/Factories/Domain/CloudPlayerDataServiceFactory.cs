using Sources.Infrastructure.Services;
using Sources.InfrastructureInterfaces;
#if YANDEX_CODE
using Sources.Infrastructure.Adapters;
using Sources.Infrastructure.Yandex;
#endif

namespace Sources.Infrastructure.Factories.Domain
{
	public class CloudPlayerDataServiceFactory
	{
		public ICloudServiceSdkFacade Create()
		{
#if YANDEX_CODE
			return new YandexCloudAdapter(new YandexServiceSdkFacade());
#endif
			return new UnityCloudServiceSdkFacade();
		}
	}
}