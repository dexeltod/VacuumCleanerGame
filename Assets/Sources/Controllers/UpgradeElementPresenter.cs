using Sources.Controllers.Common;
using Sources.ControllersInterfaces.Common;

namespace Sources.Controllers
{
	public interface IUpgradeElementPresenter : IPresenter
	{
		void Upgrade(string idName);
	}

	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		public void Upgrade(string idName)
		{
			
		}
	}
}