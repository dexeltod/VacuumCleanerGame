using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Boot.Scripts.Factories.Progress;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.States;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using VContainer;
using VContainer.Unity;

namespace Sources.Boot.Scripts
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly IGameStateChanger _gameStateChanger;
		private readonly IProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IUpdatablePersistentProgressService _updatablePersistentProgressService;

		[Inject]
		public GameBuilder(
			IProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IGameStateChanger gameStateChangerProvider,
			IGameStateChangerFactory gameStateChangerFactory,
			IUpdatablePersistentProgressService updatablePersistentProgressService
		)
		{
			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameStateChanger = gameStateChangerProvider ??
			                    throw new ArgumentNullException(nameof(gameStateChangerProvider));

			_updatablePersistentProgressService =
				updatablePersistentProgressService ?? throw new ArgumentNullException(nameof(updatablePersistentProgressService));
		}

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();

			DOTween.Init();

			_gameStateChanger.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			await _saveLoader.Initialize();

			_updatablePersistentProgressService.Update(await _progressFactory.Create());
		}
	}
}
