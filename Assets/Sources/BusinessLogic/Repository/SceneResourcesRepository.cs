using System;
using System.Collections.Generic;
using System.Linq;
using Sources.ControllersInterfaces.Services;
using Sources.DomainInterfaces.Entities;

namespace Sources.BusinessLogic.Repository
{
	public class SceneResourcesRepository : IRepository<ISceneResourceEntity>
	{
		private readonly ICollection<ISceneResourceEntity> _entities = new List<ISceneResourceEntity>();

		public void Add(ISceneResourceEntity value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			if (_entities.Any(x => x.ID == value.ID))
				throw new ArgumentException($"Entity with id {value.ID} already exists");

			_entities.Add(value);
		}

		public void AddRange(IEnumerable<ISceneResourceEntity> values)
		{
			throw new NotImplementedException();
		}

		public void DisableAll()
		{
			throw new NotImplementedException();
		}

		public void EnableMany()
		{
			throw new NotImplementedException();
		}

		public TI Get<TI>() where TI : ISceneResourceEntity => throw new NotImplementedException();

		public void Remove(ISceneResourceEntity value)
		{
			throw new NotImplementedException();
		}

		public void RemoveAll()
		{
			throw new NotImplementedException();
		}

		public void Disable<TI>()
		{
			throw new NotImplementedException();
		}

		public void Enable<TI>()
		{
			throw new NotImplementedException();
		}

		public void EnableMany(IEnumerable<ISceneResourceEntity> exclude)
		{
			throw new NotImplementedException();
		}

		public void EnableMany(Type[] exclude)
		{
			throw new NotImplementedException();
		}

		public ISceneResourceEntity Get(int id)
		{
			return _entities.FirstOrDefault(x => x.ID == id);
		}
	}
}