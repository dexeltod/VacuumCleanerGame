using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.PresentationInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IUpgradeWindowViewFactory : IPresentableFactory<IUpgradeWindow, IUpgradeWindowPresenter> { }
}