using Sources.Infrastructure.Adapters;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Yandex;
using Sources.InfrastructureInterfaces;

namespace Sources.Infrastructure.Factories.Domain
{
	public class CloudPlayerDataServiceFactory
	{
		public ICloudPlayerDataService Create()
		{
#if YANDEX_CODE
			return new YandexCloudPlayerDataAdapter(new YandexServiceSdkFacade());
#endif
			//TODO: add other platforms
			return new UnityCloudPlayerDataService();
		}
	}
}