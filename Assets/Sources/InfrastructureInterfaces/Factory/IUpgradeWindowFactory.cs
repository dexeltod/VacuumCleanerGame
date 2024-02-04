using Sources.InfrastructureInterfaces.Common;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.PresentationInterfaces;
using Sources.PresentersInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IUpgradeWindowFactory : IPresentableFactory<IUpgradeWindow, IUpgradeWindowPresenter> { }
}