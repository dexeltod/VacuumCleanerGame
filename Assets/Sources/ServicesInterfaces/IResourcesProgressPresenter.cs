using System;
using Sources.DIService;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IResourcesProgressPresenter : IResourceMaxScore, IService
	{
		IResource<int> SoftCurrency { get; }
		event Action<int> ScoreChanged;
		event Action<int> MoneyChanged;
	
		bool TryAddSand(int newScore);
		void SellSand();
		void AddMoney(int count);
		void DecreaseMoney(int count);
		int GetDecreasedMoney(int count);
		event Action<int> GlobalScoreChanged;
	}
}