using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;

namespace ViewModel.Infrastructure.Services.Factories
{
	public class GameProgressFactory
	{
		public async UniTask<GameProgressModel> CreateProgress()
		{
			//TODO Need create game progress;
			return new GameProgressModel();
		}
	}
}