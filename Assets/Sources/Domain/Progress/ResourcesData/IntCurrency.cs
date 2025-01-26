namespace Sources.Domain.Progress.ResourcesData
{
	// [Serializable]
	// public class IntCurrency : Resource<int>
	// {
	// 	public IntCurrency(int id, string name, int value, int maxValue) : base(id, name, value, maxValue)
	// 	{
	// 		if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
	// 	}
	//
	// 	public override int Value
	// 	{
	// 		get => _value;
	// 		set => Set(value);
	// 	}
	//
	// 	public override int MaxValue { get; set; }
	// 	public override event Action Changed;
	//
	// 	public void Set(int value)
	// 	{
	// 		if (value < 0 || value > MaxValue) throw new ArgumentOutOfRangeException(nameof(value));
	//
	// 		Value = value;
	// 		Changed?.Invoke();
	// 	}
	// }
}