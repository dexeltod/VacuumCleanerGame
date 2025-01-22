using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ControllersInterfaces
{
	public interface IResourcesProgressPresenter : IPresenter
	{
		bool IsMaxScoreReached { get; }
		IReadOnlyProgress<int> SoftCurrency { get; }
		void AddMoney(int count);
		void ClearTotalResources();
		int GetCalculatedDecreasedMoney(int count);

		bool TryAddSand(int newScore);
		bool TryDecreaseMoney(int count);
	}
}
