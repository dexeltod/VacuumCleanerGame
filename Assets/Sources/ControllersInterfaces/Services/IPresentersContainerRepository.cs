using System.Collections.Generic;

namespace Sources.ControllersInterfaces.Services
{
	public interface IPresentersContainerRepository
	{
		void Add(IPresenter presenter);
		void AddRange(IEnumerable<IPresenter> presenters);
		void DisableAll();
		void EnableAll();
		IPresenter Get<T>() where T : IPresenter;
		void Remove(IPresenter presenter);
		void RemoveAll();
	}
}