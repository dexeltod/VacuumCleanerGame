
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IResourcesProgressPresenter : IResourceMaxScore
	{
		IResourceReadOnly<int> SoftCurrency { get; }

		bool TryAddSand(int newScore);
		void ClearScores();
		void SellSand();
		void AddMoney(int count);
		void DecreaseMoney(int count);
		int GetDecreasedMoney(int count);
	}
}