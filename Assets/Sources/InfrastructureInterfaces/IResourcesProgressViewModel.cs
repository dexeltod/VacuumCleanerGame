using System;
using DomainInterfaces.Money;

namespace InfrastructureInterfaces
{
	public interface IResourcesProgressViewModel : IService
	{
		IResource<int> SoftCurrency { get; }
		event Action<int> ScoreChanged;
		event Action<int> MoneyChanged;
		bool CheckMaxScore();
		bool AddSand(int newScore);
		void SellSand();
		void AddMoney(int count);
		void DecreaseMoney(int count);
	}
}