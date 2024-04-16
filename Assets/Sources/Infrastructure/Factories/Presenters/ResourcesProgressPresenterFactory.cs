using System;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Services;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class ResourcesProgressPresenterFactory : PresenterFactory<ResourcesProgressPresenter>
	{
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IPersistentProgressServiceProvider _persistentProgressService;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly ISandParticleSystemProvider _sandParticleSystemProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;
		private readonly PlayerStatsNames _playerStatsNames;

		private IResourcesModel ResourcesModel =>
			_persistentProgressService.Implementation.GlobalProgress.ResourcesModel;

		private IGameProgress UpgradeProgressModel =>
			_persistentProgressService.Implementation.GlobalProgress.UpgradeProgressModel;

		[Inject]
		public ResourcesProgressPresenterFactory(
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IPersistentProgressServiceProvider persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			ISandParticleSystemProvider sandParticleSystemProvider,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			PlayerStatsNames playerStatsNames
		)
		{
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticleSystemProvider = sandParticleSystemProvider ??
				throw new ArgumentNullException(nameof(sandParticleSystemProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
			_playerStatsNames = playerStatsNames ?? throw new ArgumentNullException(nameof(playerStatsNames));
		}

		public override ResourcesProgressPresenter Create()
		{
			var sandParticlePlayerSystem
				= new SandParticlePlayerSystem(
					_sandParticleSystemProvider,
					_coroutineRunnerProvider,
					playTime: 1
				);

			return new ResourcesProgressPresenter(
				_gameplayInterfacePresenterProvider,
				ResourcesModel,
				_fillMeshShaderControllerProvider,
				sandParticlePlayerSystem,
				UpgradeProgressModel,
				_playerStatsServiceProvider,
				_playerStatsNames
			);
		}
	}
}