using System;
using System.Collections.Generic;

namespace Sources.BusinessLogic.Repository
{
	public interface IActiveRepository<in T> : IRepository<T>
	{
		void Disable<TI>();
		void DisableAll();
		void Enable<TI>();
		void EnableMany();
		void EnableMany(IEnumerable<T> excluded);
		void EnableMany(Type[] excluded);
	}
}
