using System;

namespace ViewModel.Infrastructure.Services.DataViewModel
{
	public interface IResourcesProgressViewModel : IService
	{
		int Money { get; }
		event Action<int> ScoreChanged;
		event Action<int> MoneyChanged;
		bool CheckMaxScore();
		bool AddSand(int newScore);
		void SellSand();
		void AddMoney(int count);
		void DecreaseMoney(int count);
	}
}