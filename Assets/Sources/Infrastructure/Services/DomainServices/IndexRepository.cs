using System;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Infrastructure.Services.DomainServices
{
	public class IndexRepository<T> : IIndexRepository<T> where T : IResource<T>
	{
		private readonly Dictionary<int, T> _entities;

		public IndexRepository(Dictionary<int, T> entities) =>
			_entities = entities ?? throw new ArgumentNullException(nameof(entities));

		public void Add(int id, T value)
		{
			if (!_entities.TryAdd(id, value))
				throw new InvalidOperationException($"Entity with ID {id} already exists.");
		}

		public void AddRange(IEnumerable<T> values)
		{
			foreach (T value in values)
			{
				if (_entities.ContainsKey(value.Id))
					throw new InvalidOperationException($"Entity with ID {value.Id} already exists.");

				Add(value.Id, value);
			}
		}

		public T Get(int id)
		{
			_entities.TryGetValue(id, out T value);

			if (value == null)
				throw new InvalidOperationException($"Entity with ID {id} not found.");

			return value;
		}

		public void Remove(int id)
		{
			if (_entities.ContainsKey(id) == false)
				throw new InvalidOperationException($"Entity with ID {id} not found.");

			_entities.Remove(id);
		}

		public void RemoveAll() => _entities.Clear();
	}
}
