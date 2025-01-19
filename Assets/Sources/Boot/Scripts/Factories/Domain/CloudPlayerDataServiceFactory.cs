using Sources.BusinessLogic.Interfaces;
using Sources.Infrastructure.Services;
#if YANDEX_CODE
using Sources.Infrastructure.Adapters;
using Sources.Infrastructure.Yandex;
#endif

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class CloudPlayerDataServiceFactory
	{
		public ICloudServiceSdk Create()
		{
#if YANDEX_CODE
			return new YandexCloudAdapter(new YandexServiceSdkFacade());
#endif
			return new UnityCloudServiceSdk();
		}
	}
}