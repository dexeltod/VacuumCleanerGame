using System;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class IntCurrency : Resource<int>
	{
		private int _value;

		public IntCurrency(int id, string name, int value) : base(id, name, value)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
		}
	}
}
