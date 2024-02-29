using Sources.ApplicationServicesInterfaces;

namespace Sources.Infrastructure.StateMachine.GameStates
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