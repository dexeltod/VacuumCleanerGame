using System;
using Sources.Application.Utils;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class Resource : IResource<int>
	{
		private int _count;
		
		public Resource(ResourceType resourceType)
		{
			ResourceType = resourceType;
		}

		public ResourceType ResourceType { get; }

		public int Count
		{
			get => _count;
			set
			{
				var oldCount = _count;

				if (value < 0)
					throw new Exception("New count is negative");

				if (value == oldCount)
					return;

				_count = value;

				CountChanged?.Invoke(_count);
			}
		}

		public event Action<int> CountChanged;
	}
}