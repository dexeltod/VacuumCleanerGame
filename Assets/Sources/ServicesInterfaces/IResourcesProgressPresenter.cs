using System;
using Sources.DIService;
using Sources.Domain.Progress.ResourcesData;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IResourcesProgressPresenter : IService
	{
		IResource<int> SoftCurrency { get; }
		event Action<int> ScoreChanged;
		event Action<int> MoneyChanged;
		bool CheckMaxScore();
		bool AddSand(int newScore);
		void SellSand();
		void AddMoney(int count);
		void DecreaseMoney(int count);
		int GetDecreasedMoney(int count);
	}
}