using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Unity.Services.Core;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class SaveLoaderFactory
	{
		public ISaveLoader Create()
		{
#if !YANDEX_CODE

			var saveLoader = new UnitySaveLoader(
				new UnityServicesOptions(new InitializationOptions()),
				new UnityCloudSaveLoaderLoader()
			);
			return saveLoader;
#endif
#if YANDEX_CODE
				new YandexCloudSaveLoader();
#endif

#if YANDEX_CODE
			return new YandexSaveLoader(_cloudSave);
#endif
		}
	}
}