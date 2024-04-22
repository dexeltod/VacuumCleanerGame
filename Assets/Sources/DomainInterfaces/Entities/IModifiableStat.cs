namespace Sources.Domain.Temp
{
	public interface IModifiableStat
	{
		void Increase(int value);
		void Decrease(int value);
		void Set(int value);
		void Clear();
		int Value { get; }
	}
}