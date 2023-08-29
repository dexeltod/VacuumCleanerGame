using System;
using Sources.Application.Utils;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class IntResource : IResource<int>
	{
		private int _count;
		public int Count => _count;

		public ResourceType ResourceType { get; }
		public event Action<int> ResourceChanged;

		public IntResource(ResourceType resourceType)
		{
			ResourceType = resourceType;
		}

		public void Set(int value)
		{
			if (SeeExceptions(value)) return;

			_count = value;

			ResourceChanged?.Invoke(_count);
		}

		public void Increase(int value)
		{
			if (SeeExceptions(value)) return;

			_count += value;

			ResourceChanged?.Invoke(_count);
		}

		public void Decrease(int value)
		{
			if (SeeExceptions(value)) return;

			_count -= value;

			ResourceChanged?.Invoke(_count);
		}

		private bool SeeExceptions(int value)
		{
			int oldCount = _count;

			if (value < 0)
				throw new Exception("New count is negative");

			if (value == oldCount)
				return true;
			return false;
		}
	}
}