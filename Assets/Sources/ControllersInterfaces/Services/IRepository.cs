using System;
using System.Collections.Generic;

namespace Sources.ControllersInterfaces.Services
{
	public interface IRepository<in T>
	{
		void Add(T value);
		void AddRange(IEnumerable<T> values);
		void Disable<TI>();
		void DisableAll();
		void Enable<TI>();
		void EnableMany();
		void EnableMany(IEnumerable<T> exclude);
		void EnableMany(Type[] exclude);
		TI Get<TI>() where TI : T;
		void Remove(T value);
		void RemoveAll();
	}
}