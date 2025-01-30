using System.Collections.Generic;

namespace Sources.BusinessLogic.Repository
{
	public interface IRepository<in T>
	{
		void Add(T value);
		void AddRange(IEnumerable<T> values);
		TI Get<TI>() where TI : T;
		void Remove(T value);
		void RemoveAll();
	}
}