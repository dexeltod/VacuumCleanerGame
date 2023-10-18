using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils.Configs;

namespace Sources.Infrastructure
{
	public class LevelProgressPresenter : ILevelProgressPresenter
	{
		private const string CurrentLevelName = "CurrentLevel";
		private const int    OnePoint         = 1;

		private readonly IGameProgress          _progressService;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		public int CurrentLevelNumber { get; private set; }

		public LevelProgressPresenter
		(
			IGameProgress          progressService,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_progressService       = progressService;
			_gameplayInterfaceView = gameplayInterfaceView;
		}

		public void SetNextLevel(int newValue)
		{
			IUpgradeProgressData progressData = _progressService.GetByName(CurrentLevelName);

			int newLevel = progressData.Value + OnePoint;
			CurrentLevelNumber = newLevel;

			_progressService.SetProgress(CurrentLevelName, newLevel);

			_gameplayInterfaceView.SetCurrentLevel(CurrentLevelNumber);
			_gameplayInterfaceView.SetActiveGoToNextLevelButton(false);
		}
	}
}