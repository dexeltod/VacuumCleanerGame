using System;
using System.Collections.Generic;
using System.Linq;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;

namespace Sources.Controllers.Services
{
	public class PresentersRepository : IRepository<IPresenter>
	{
		private readonly Dictionary<Type, IPresenter> _presenters = new();

		public void Add(IPresenter value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			if (_presenters.ContainsKey(value.GetType())) return;

			_presenters.Add(value.GetType(), value);
		}

		public T Get<T>() where T : IPresenter => (T)_presenters.GetValueOrDefault(typeof(T));

		public void AddRange(IEnumerable<IPresenter> values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));

			foreach (IPresenter presenter in values) Add(presenter);
		}

		public void Remove(IPresenter value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			_presenters.Remove(value.GetType());
		}

		public void RemoveAll() => _presenters.Clear();

		public void EnableMany()
		{
			foreach (IPresenter presenter in _presenters.Values) presenter.Enable();
		}

		public void EnableMany(Type[] exclude)
		{
			if (exclude == null) throw new ArgumentNullException(nameof(exclude));

			foreach (IPresenter value in _presenters.Values)
			{
				if (exclude.Contains(value.GetType()))
				{
					value.Disable();
					continue;
				}

				value.Enable();
			}
		}

		public void EnableMany(IEnumerable<IPresenter> exclude)
		{
			if (exclude == null) throw new ArgumentNullException(nameof(exclude));

			IEnumerable<IPresenter> presenters = _presenters.Values.Select(
				presenter =>
				{
					if (!_presenters.Values.Contains(presenter)) ;
					return presenter;
				}
			);

			foreach (IPresenter presenter in presenters)
				presenter.Enable();
		}

		public void Enable<T>()
		{
			if (!_presenters.TryGetValue(typeof(T), out IPresenter presenter)) throw new ArgumentNullException(nameof(T));

			presenter.Enable();
		}

		public void Disable<T>()
		{
			if (!_presenters.TryGetValue(typeof(T), out IPresenter presenter)) throw new ArgumentNullException(nameof(T));

			presenter.Disable();
		}

		public void DisableAll()
		{
			foreach (IPresenter presenter in _presenters.Values) presenter.Disable();
		}
	}
}