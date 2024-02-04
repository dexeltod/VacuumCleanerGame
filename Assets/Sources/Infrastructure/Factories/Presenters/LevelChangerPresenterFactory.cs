using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Common.Decorators;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class LevelChangerPresenterFactory : PresenterFactory<LevelChangerPresenter>
	{
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IResourcesProgressPresenter _progressPresenter;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly IAdvertisement _advertisement;

		public LevelChangerPresenterFactory(
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			IResourcesProgressPresenter progressPresenter,
			IProgressLoadDataService progressLoadDataService,
			IAdvertisement advertisement
		)
		{
			_levelProgressFacade = levelProgressFacade;
			_gameStateMachine = gameStateMachine;
			_levelConfigGetter = levelConfigGetter;
			_progressPresenter = progressPresenter;
			_progressLoadDataService = progressLoadDataService;
			_advertisement = advertisement;
		}

		public override LevelChangerPresenter Create() =>
			new(
				_levelProgressFacade,
				_gameStateMachine,
				_levelConfigGetter,
				_progressPresenter,
				_progressLoadDataService,
				_advertisement
			);
	}
}