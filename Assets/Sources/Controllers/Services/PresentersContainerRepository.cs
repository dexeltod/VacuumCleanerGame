using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;

namespace Sources.Controllers.Services
{
	public class PresentersContainerRepository : IPresentersContainerRepository
	{
		private readonly ICollection<IPresenter> _presenters = new List<IPresenter>();

		public void Add(IPresenter presenter)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));

			_presenters.Add(presenter);
		}

		public void AddRange(IEnumerable<IPresenter> presenters)
		{
			if (presenters == null) throw new ArgumentNullException(nameof(presenters));

			foreach (var presenter in presenters) Add(presenter);
		}

		public void Remove(IPresenter presenter)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));

			_presenters.Remove(presenter);
		}

		public void RemoveAll() =>
			_presenters.Clear();

		public void EnableAll()
		{
			foreach (var presenter in _presenters) presenter.Enable();
		}

		public void DisableAll()
		{
			foreach (var presenter in _presenters) presenter.Disable();
		}
	}
}
