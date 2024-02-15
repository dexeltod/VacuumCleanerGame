using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services.DomainServices;
using Unity.Services.Core;
using VContainer;

namespace Sources.Infrastructure.Factories
{
	public class SaveLoaderFactory
	{
		private readonly IPersistentProgressServiceProvider _progressService;
		private readonly ICloudSave _cloudSave;

		[Inject]
		public SaveLoaderFactory(
			IPersistentProgressServiceProvider progressService,
			ICloudSave cloudSave
		)
		{
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_cloudSave = cloudSave ?? throw new ArgumentNullException(nameof(cloudSave));
		}

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

			return new UnitySaveLoader(_progressService, controller, _cloudSave);
		}
	}
}