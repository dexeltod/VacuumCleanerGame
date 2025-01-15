using System;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class Float : Currency<float>
	{
		public Float(int id, string name, float value) :
			base(id, name, value)
		{
		}
	}
}