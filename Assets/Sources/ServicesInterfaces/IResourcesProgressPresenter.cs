using Sources.DIService;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface IResourcesProgressPresenter : IResourceMaxScore, IService
	{
		IResource<int>    SoftCurrency { get; }

		bool               TryAddSand(int newScore);
		void               SellSand();
		void               AddMoney(int          count);
		void               DecreaseMoney(int     count);
		int                GetDecreasedMoney(int count);
		
	}
}