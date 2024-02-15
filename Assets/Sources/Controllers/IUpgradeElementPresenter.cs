using Sources.ControllersInterfaces.Common;

namespace Sources.Controllers
{
	public interface IUpgradeElementPresenter : IPresenter
	{
		void Upgrade(string progressIdName);
	}
}