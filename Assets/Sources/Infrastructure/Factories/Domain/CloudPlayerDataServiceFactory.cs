using Sources.ApplicationServicesInterfaces;
using Sources.Infrastructure.Yandex;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public class CloudPlayerDataServiceFactory
	{
		public ICloudPlayerDataService Create()
		{
#if YANDEX_CODE
			return new YandexCloudPlayerDataAdapter(new YandexServiceSdkFacade());
#endif
		}
	}
}