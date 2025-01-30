using System.Collections.Generic;

namespace Sources.BusinessLogic.Repository
{
	public interface IIndexRepository<T>
	{
		void Add(int id, T value);
		void AddRange(IEnumerable<T> values);
		T Get(int id);
		void Remove(int id);
		void RemoveAll();
	}
}
