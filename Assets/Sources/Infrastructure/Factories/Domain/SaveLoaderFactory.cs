using Sources.BusinessLogic.Interfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services.DomainServices;
using Unity.Services.Core;

namespace Sources.Infrastructure.Factories.Domain
{
	public class SaveLoaderFactory
	{
		private readonly ICloudSaveLoader _cloudSaveLoader;

		public ISaveLoader Create()
		{
#if !YANDEX_CODE
			var cloudSaveLoader = new UnityCloudSaveLoaderLoader();
			return GetEditorSaveLoader();
#endif
#if YANDEX_CODE
				new YandexCloudSaveLoader();
#endif

#if YANDEX_CODE
			return new YandexSaveLoader(_cloudSave);
#endif
		}

		private UnitySaveLoader GetEditorSaveLoader()
		{
			UnityServices controller = new UnityServices(new InitializationOptions());

			return new UnitySaveLoader(controller, _cloudSaveLoader);
		}
	}
}
