namespace Model
{
	public class Game
	{
		private readonly MusicSetter _musicSetter;
		public readonly GameStateMachine StateMachine;

		public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, MusicSetter musicSetter)
		{
			_musicSetter = musicSetter;
			StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain,
				ServiceLocator.Container,
				_musicSetter);
		}
	}
}