using System;
using Sources.Utils;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable] public class IntResource : Resource<int>
	{
		public IntResource(ResourceType resourceType) : base(resourceType) { }
	}
}