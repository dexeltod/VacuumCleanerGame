using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;

namespace Sources.Controllers.Services
{
	public class PresentersContainerRepository : IPresentersContainerRepository
	{
		private readonly Dictionary<Type, IPresenter> _presenters = new();

		public void Add(IPresenter presenter)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));

			_presenters[presenter.GetType()] = presenter;
		}

		public IPresenter Get<T>() where T : IPresenter =>
			_presenters.GetValueOrDefault(typeof(T));

		public void AddRange(IEnumerable<IPresenter> presenters)
		{
			if (presenters == null) throw new ArgumentNullException(nameof(presenters));

			foreach (IPresenter presenter in presenters) Add(presenter);
		}

		public void Remove(IPresenter presenter)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));

			_presenters.Remove(presenter.GetType());
		}

		public void RemoveAll() =>
			_presenters.Clear();

		public void EnableAll()
		{
			foreach (IPresenter presenter in _presenters.Values) presenter.Enable();
		}

		public void DisableAll()
		{
			foreach (IPresenter presenter in _presenters.Values) presenter.Disable();
		}
	}
}