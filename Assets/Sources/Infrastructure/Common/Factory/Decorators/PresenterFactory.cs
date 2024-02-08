using System.Collections.Generic;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces.Common;
using Sources.InfrastructureInterfaces.Common.Factories.Decorators;

namespace Sources.Infrastructure.Common.Factory.Decorators
{
	public abstract class PresenterFactory<T> : IPresenterFactory<T> where T : Presenter, IPresenter
	{
		public abstract T Create();
	}
}