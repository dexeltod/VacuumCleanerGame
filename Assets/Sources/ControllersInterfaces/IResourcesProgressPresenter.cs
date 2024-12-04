using Sources.ControllersInterfaces.Common;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ControllersInterfaces
{
	public interface IResourcesProgressPresenter : IPresenter
	{
		bool IsMaxScoreReached { get; }
		IReadOnlyProgressValue<int> SoftCurrency { get; }

		bool TryAddSand(int newScore);
		void ClearTotalResources();
		void Sell();
		void AddMoney(int count);
		int GetCalculatedDecreasedMoney(int count);
		bool TryDecreaseMoney(int count);
	}
}