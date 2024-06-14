namespace Sources.DomainInterfaces.Entities
{
	public interface IStat : IStatReadOnly
	{
		void Set(float value);
		void Increase(float value);
		void Decrease(float value);
		void SetDefault();
		void Enable();
		void Disable();
	}
}