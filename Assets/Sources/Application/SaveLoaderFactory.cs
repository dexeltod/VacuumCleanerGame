using System;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.DomainServices;
using Unity.Services.Core;
using VContainer;

namespace Sources.Application
{
	public class SaveLoaderFactory
	{
		private readonly IYandexSDKController _sdkController;
		private readonly IPersistentProgressService _progressService;

		[Inject]
		public SaveLoaderFactory(
			IYandexSDKController sdkController,
			IPersistentProgressService
				progressService
		)
		{
			_sdkController = sdkController;
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
		}

		public ISaveLoader GetSaveLoader()
		{
#if YANDEX_GAMES && YANDEX_CODE
			return new YandexSaveLoader(sdkController);
#endif

#if UNITY_EDITOR
			return GetEditorSaveLoader();
#endif
		}

		private EditorSaveLoader GetEditorSaveLoader()
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());
			controller.InitializeUnityServices();

			return new EditorSaveLoader(_progressService, controller);
		}
	}
}