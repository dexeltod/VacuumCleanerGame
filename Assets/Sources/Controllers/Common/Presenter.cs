using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.ViewEntities;

namespace Sources.Controllers.Common
{
	public abstract class Presenter : IPresenter
	{
		private readonly IViewEntity _viewEntity;

		protected Presenter(IViewEntity viewEntity = null) => _viewEntity = viewEntity;

		public virtual void Enable()
		{
			_viewEntity?.SetActive(true);
		}

		public virtual void Disable()
		{
			_viewEntity?.SetActive(false);
		}
	}
}
