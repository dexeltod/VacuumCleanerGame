using System;

namespace Model
{
	public class MenuState : IGameState
	{
		private const string MainMenu = "Main_Menu";
		
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IMusicService _musicService;

		public MenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
		{
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_musicService = ServiceLocator.Container.GetSingle<IMusicService>();
		}

		public void Enter()
		{
			_musicService.Set(ConstantNames.MusicNames.MenuMusic);
			_sceneLoader.Load(MainMenu);
		}
		public void Exit()
		{
			_loadingCurtain.gameObject.SetActive(true);
		}
	}
}