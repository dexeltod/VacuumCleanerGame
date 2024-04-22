using System;
using Sources.ControllersInterfaces;
using Sources.Domain.Progress.Player;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.DataViewModel
{
	public class ProgressSetterFacade : IProgressSetterFacade
	{
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly IProgressService _progressService;
		private readonly IPersistentProgressService _persistentProgressService;

		public ProgressSetterFacade(
			IProgressSaveLoadDataService persistentProgressService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			IProgressService progressService
		)
		{
			_progressSaveLoadDataService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
		}

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public bool TryAddOneProgressPoint(int id)
		{
			IResourceModelModifiable resourceModel = _persistentProgressServiceProvider.Implementation.GlobalProgress
				.ResourceModelReadOnly as IResourceModelModifiable;

			resourceModel!.DecreaseMoney(_progressService.GetPrice(id));
			_progressService.AddProgressPoint(id);

			_progressSaveLoadDataService.SaveToCloud();

			return true;
		}
	}
}