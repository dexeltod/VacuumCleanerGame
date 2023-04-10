using Cysharp.Threading.Tasks;

namespace Model
{
	public class GameProgressFactory
	{
		public async UniTask<GameProgress> CreateProgress()
		{
			//TODO Need create game progress;
			return new GameProgress();
		}
	}
}