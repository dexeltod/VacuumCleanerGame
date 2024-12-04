using Sources.ControllersInterfaces.Common;

namespace Sources.Controllers.Common
{
	public abstract class Presenter : IPresenter
	{
		public virtual void Enable() { }

		public virtual void Disable() { }
	}
}