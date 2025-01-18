using Sources.DomainInterfaces;

namespace Sources.ControllersInterfaces.Factories
{
	public interface IMainMenuPresenterFactory
	{
		IMainMenuPresenter Create(IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory);
	}
}
