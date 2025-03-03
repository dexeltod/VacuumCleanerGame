using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Repository;
using Sources.ControllersInterfaces;

namespace Sources.Controllers.Services
{
	public class PresentersRepository : IActiveRepository<IPresenter>
	{
		private readonly Dictionary<Type, IPresenter> _presenters = new();

		public void Add(IPresenter value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			if (_presenters.ContainsKey(value.GetType())) return;

			_presenters.Add(value.GetType(), value);
		}

		public T Get<T>() where T : IPresenter
		{
			var presenter = (T)_presenters.GetValueOrDefault(typeof(T));

			if (presenter == null) throw new InvalidOperationException($"No presenter of type {typeof(T)} registered.");

			return presenter;
		}

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

		public void RemoveAll()
		{
			if (_presenters.Count > 0)
				_presenters.Clear();
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

		public void Enable<T>()
		{
			if (!_presenters.TryGetValue(typeof(T), out IPresenter presenter)) throw new ArgumentNullException(nameof(T));

			presenter.Enable();
		}

		public void EnableMany()
		{
			foreach (IPresenter presenter in _presenters.Values) presenter.Enable();
		}

		public void EnableMany(Type[] excluded)
		{
			if (excluded == null) throw new ArgumentNullException(nameof(excluded));

			foreach (IPresenter value in _presenters.Values)
			{
				if (excluded.Contains(value.GetType())) continue;

				value.Enable();
			}
		}

		public void EnableMany(IEnumerable<IPresenter> excluded)
		{
			if (excluded == null) throw new ArgumentNullException(nameof(excluded));

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
	}
}
