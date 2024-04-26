namespace Sources.Domain.Temp
{
	public interface IStatChangeable
	{
		void Set(float value);
		void Increase(float value);
		void Decrease(float value);
		void Clear();
		void Enable();
		void Disable();
	}
}