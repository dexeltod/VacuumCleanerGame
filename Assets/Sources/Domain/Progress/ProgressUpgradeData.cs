using System;
using Sources.DomainInterfaces;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ProgressUpgradeData : IUpgradeProgressData
	{
		private int _value;
		public string Name { get; }

		public int Value
		{
			get => _value;
			set
			{
				if (value < 0)
					throw new Exception("Value is negative");

				_value = value;
			}
		}

		public ProgressUpgradeData(string name, int pointCount)
		{
			Name = name;
			Value = pointCount;
		}
	}
}