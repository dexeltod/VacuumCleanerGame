using System;
using Sources.Application.Utils;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class FloatResource : IResource<float>
	{
		private float _count;

		public float Count => _count;
		public ResourceType ResourceType { get; }

		public event Action<float> ResourceChanged;

		public FloatResource(ResourceType resourceType)
		{
			ResourceType = resourceType;
		}

		public void Set(float value)
		{
			if (SeeExceptions(value)) return;

			_count = value;

			ResourceChanged?.Invoke(_count);
		}

		public void Increase(float value)
		{
			if (SeeExceptions(value)) return;

			_count += value;

			ResourceChanged?.Invoke(_count);
		}

		public void Decrease(float value)
		{
			if (SeeExceptions(value)) return;

			_count -= value;

			ResourceChanged?.Invoke(_count);
		}

		private bool SeeExceptions(float value)
		{
			float oldCount = _count;

			if (value < 0)
				throw new Exception("New count is negative");

			if (value == oldCount)
				return true;
			return false;
		}
	}
}