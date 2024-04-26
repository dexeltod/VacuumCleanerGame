using System;
using Sources.Domain.Progress.ResourcesData;

namespace Sources.Domain.Progress.Entities.Values
{
	[Serializable] public class IntEntityValue : Resource<int>
	{
		public IntEntityValue(int count) : base(count) { }
	}
}