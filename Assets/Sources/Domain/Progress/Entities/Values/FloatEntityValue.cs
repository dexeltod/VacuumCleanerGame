using Sources.Domain.Progress.ResourcesData;

namespace Sources.Domain.Progress.Entities.Values
{
	public class FloatEntityValue : Resource<float>
	{
		public FloatEntityValue(float value) : base(value) { }
	}
}