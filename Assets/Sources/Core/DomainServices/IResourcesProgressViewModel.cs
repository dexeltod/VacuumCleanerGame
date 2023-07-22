using System;
using Sources.Core;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure.InfrastructureInterfaces
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