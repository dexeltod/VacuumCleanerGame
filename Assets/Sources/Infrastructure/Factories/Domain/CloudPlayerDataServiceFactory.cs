using Sources.Infrastructure.Services;

#if YANDEX_CODE
using Sources.Infrastructure.Adapters;
using Sources.Infrastructure.Yandex;
#endif
using Sources.InfrastructureInterfaces;

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