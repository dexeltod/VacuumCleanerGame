using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.BusinessLogic.Repository
{
	public interface IResourcesProgressPresenter : IPresenter
	{
		IReadOnlyProgress<int> SoftCurrency { get; }
		void ClearTotalResources();
	}
}