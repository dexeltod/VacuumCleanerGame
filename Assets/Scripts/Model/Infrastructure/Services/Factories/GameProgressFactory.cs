using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;

namespace Model.Infrastructure.Services.Factories
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