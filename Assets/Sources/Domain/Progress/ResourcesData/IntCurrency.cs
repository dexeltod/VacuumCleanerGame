using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.Domain.Progress.ResourcesData
{
	[Serializable]
	public class IntCurrency : IResource<int>, IReadOnlyProgress<int>
	{
		private int _id;
		private int _value;
		private string _name;

		public IntCurrency(int id, string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			_name = name;
		}

		public event Action Changed;

		public int Value
		{
			get => _value;
			set
			{
				_value = value;
				Changed?.Invoke();
			}
		}

		public string Name => _name;
		public int Id => _id;

		public void Set(int newValue)
		{
			if (newValue < 0) throw new ArgumentOutOfRangeException(nameof(newValue));

			Value = newValue;
		}
	}
}
