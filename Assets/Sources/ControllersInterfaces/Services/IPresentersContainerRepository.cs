using System.Collections.Generic;

namespace Sources.ControllersInterfaces.Services
{
	public interface IPresentersContainerRepository
	{
		void Add(IPresenter presenter);
		void Remove(IPresenter presenter);
		void EnableAll();
		void DisableAll();
		void RemoveAll();
		void AddRange(IEnumerable<IPresenter> presenters);
	}
}
