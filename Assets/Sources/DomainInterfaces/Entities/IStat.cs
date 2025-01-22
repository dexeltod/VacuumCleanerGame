namespace Sources.DomainInterfaces.Entities
{
	public interface IStat : IStatReadOnly
	{
		void Decrease(float value);
		void Disable();
		void Enable();
		void Increase(float value);
		void Set(float value);
		void SetDefault();
	}
}