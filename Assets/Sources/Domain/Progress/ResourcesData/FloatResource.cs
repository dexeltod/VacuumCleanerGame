using System;
using Sources.Utils;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class FloatResource : Resource<float>
	{
		public FloatResource(ResourceType resourceType) : base(resourceType)
		{
		}
	}
}