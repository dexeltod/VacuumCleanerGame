using System;
using Model.Infrastructure.Services;

namespace ViewModel
{
	public interface IGameProgressViewModel : IService
	{
		event Action<int> ScoreChanged;
		event Action<int> MoneyChanged;
		bool CheckMaxScore();
		bool AddSand(int newScore);
		void SellSand();
	}
}