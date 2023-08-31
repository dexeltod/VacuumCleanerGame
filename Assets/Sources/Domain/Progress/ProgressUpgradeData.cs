using System;
using Sources.DomainInterfaces;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ProgressUpgradeData : IUpgradeProgressData
	{
		[SerializeField] private int _value;
		[SerializeField] private string _name;
		
		[JsonProperty(nameof(Name))]
		public string Name => _name;

		[JsonProperty(nameof(Value))]
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

		[JsonConstructor]
		public ProgressUpgradeData(string name, int pointCount)
		{
			_name = name;
			_value = pointCount;
		}
	}
}