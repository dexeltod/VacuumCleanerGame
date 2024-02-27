using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.DomainServices;
using Unity.Services.Core;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class SaveLoaderFactory
	{
		private readonly ICloudSave _cloudSave;

		[Inject]
		public SaveLoaderFactory(ICloudSave cloudSave) =>
			_cloudSave = cloudSave ?? throw new ArgumentNullException(nameof(cloudSave));

		public ISaveLoader GetSaveLoader()
		{
#if YANDEX_CODE
			return new YandexSaveLoader(_cloudSave);
#endif

#if UNITY_EDITOR
			return GetEditorSaveLoader();
#endif
		}

		private UnitySaveLoader GetEditorSaveLoader()
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());

			return new UnitySaveLoader(controller, _cloudSave);
		}
	}
}