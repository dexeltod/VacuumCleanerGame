using System;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public abstract class Currency<T> : Resource<T>
	{
		protected Currency(int id, string name, T value) : base(id, name, value)
		{
		}
	}
}