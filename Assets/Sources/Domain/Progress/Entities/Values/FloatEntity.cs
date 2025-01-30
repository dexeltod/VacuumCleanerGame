using System;
using Sources.Domain.Progress.ResourcesData;

namespace Sources.Domain.Progress.Entities.Values
{
	public class FloatEntity : Resource<float>
	{
		public FloatEntity(int id, string name, float value, float maxValue) : base(id, name, value, maxValue)
		{
		}

		public override float Value { get; set; }
		public override float MaxValue { get; set; }

		public override event Action Changed;
		public override event Action HalfReached;
	}
}
