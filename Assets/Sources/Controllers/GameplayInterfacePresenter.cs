using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;

namespace Sources.Controllers
{
	public class GameplayInterfacePresenter : Presenter, IGameplayInterfacePresenter
	{
		private readonly ILevelChangerService _levelChangerService;

		public GameplayInterfacePresenter(ILevelChangerService levelChangerService)
		{
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
		}
		
		public void GoToNextLevel() =>
			_levelChangerService.GoNextLevelWithReward();
	}
}