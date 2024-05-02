using System;
using Sources.ControllersInterfaces;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.DataViewModel
{
	// public class ProgressSetterFacade : IProgressSetterFacade
	// {
	// 	private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
	// 	private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
	// 	private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
	// 	private readonly IProgressService _progressService;
	// 	private readonly IPlayerModelRepository _playerModelRepository;
	// 	private readonly IPersistentProgressService _persistentProgressService;
	//
	// 	public ProgressSetterFacade(
	// 		IProgressSaveLoadDataService persistentProgressService,
	// 		IPersistentProgressServiceProvider persistentProgressServiceProvider,
	// 		IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
	// 		IProgressService progressService,
	// 		IPlayerModelRepository playerModelRepository
	// 	)
	// 	{
	// 		_progressSaveLoadDataService = persistentProgressService ??
	// 			throw new ArgumentNullException(nameof(persistentProgressService));
	// 		_persistentProgressServiceProvider = persistentProgressServiceProvider ??
	// 			throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
	// 		_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
	// 			throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
	// 		_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
	// 		_playerModelRepository
	// 			= playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
	// 	}
	//
	// 	private IResourcesProgressPresenter ResourcesProgressPresenter => _resourcesProgressPresenterProvider.Self;
	//
	// 	private IResourceModelReadOnly GlobalProgressResourceModelReadOnly =>
	// 		_persistentProgressServiceProvider.Self.GlobalProgress
	// 			.ResourceModelReadOnly;
	//
	// 	public bool TryAddOneProgressPoint(int id)
	// 	{
	// 		IResourceModel resourceModel = GlobalProgressResourceModelReadOnly as IResourceModel;
	//
	// 		if (resourceModel!.TryDecreaseMoney(_progressService.GetPrice(id)) == false)
	// 			return false;
	//
	// 		SetStat(id);
	//
	// 		_progressService.AddProgressPoint(id);
	//
	// 		_progressSaveLoadDataService.SaveToCloud();
	//
	// 		return true;
	// 	}
	//
	// 	private void SetStat(int id) =>
	// 		((IStat)_playerModelRepository.Get(id))
	// 		.Set(_progressService.GetProgressStatValue(id));
	// }
}