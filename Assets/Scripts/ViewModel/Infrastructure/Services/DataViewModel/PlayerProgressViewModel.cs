using Model.Data.Player;
using Model.DI;

namespace ViewModel.Infrastructure.Services.DataViewModel
{
	public class PlayerProgressViewModel : IPlayerProgressViewModel
	{
		private readonly PlayerProgress _playerProgress;

		public PlayerProgressViewModel()
		{
			_playerProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress.PlayerProgress;
		}

		public void SetProgress(string progressName)
		{
			//TODO Need to create PlayerStats class
			
		}

		public void CheckProgressName()
		{
		}
	}
}