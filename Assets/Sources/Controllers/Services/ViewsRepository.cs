using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Repository;
using Sources.PresentationInterfaces.Common;

namespace Sources.Controllers.Services
{
	public class ViewsRepository : IRepository<IView>
	{
		private readonly Dictionary<Type, IView> _views = new();

		public void Add(IView value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			if (_views.ContainsKey(value.GetType())) return;

			_views.Add(value.GetType(), value);
		}

		public T Get<T>() where T : IView => (T)_views.GetValueOrDefault(typeof(T));

		public void AddRange(IEnumerable<IView> values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));

			foreach (IView view in values) Add(view);
		}

		public void Remove(IView value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			_views.Remove(value.GetType());
		}

		public void RemoveAll() => _views.Clear();

		public void Disable<T>()
		{
			if (!_views.TryGetValue(typeof(T), out IView view)) throw new ArgumentNullException(nameof(T));

			view.Disable();
		}

		public void DisableAll()
		{
			foreach (IView view in _views.Values) view.Disable();
		}

		public void Enable<T>()
		{
			if (!_views.TryGetValue(typeof(T), out IView view)) throw new ArgumentNullException(nameof(T));

			view.Enable();
		}

		public void EnableMany()
		{
			foreach (IView view in _views.Values) view.Enable();
		}

		public void EnableMany(Type[] excluded)
		{
			if (excluded == null) throw new ArgumentNullException(nameof(excluded));

			foreach (IView value in _views.Values)
			{
				if (excluded.Contains(value.GetType()))
				{
					value.Disable();
					continue;
				}

				value.Enable();
			}
		}

		public void EnableMany(IEnumerable<IView> excluded)
		{
			if (excluded == null) throw new ArgumentNullException(nameof(excluded));

			IEnumerable<IView> views = _views.Values.Select(
				view =>
				{
					if (!_views.Values.Contains(view)) ;
					return view;
				}
			);

			foreach (IView view in views)
				view.Enable();
		}
	}
}
