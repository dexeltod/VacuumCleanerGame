namespace Sources.DomainInterfaces.Entities
{
	public interface IStat : IStatReadOnly
	{
		void Set(float value);
		void Increase(float value);
		void Decrease(float value);
		void Clear();
		void Enable();
		void Disable();
	}
}