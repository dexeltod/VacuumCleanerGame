namespace Sources.DomainInterfaces
{
	public interface ISpeedDecorator
	{
		bool IsDecorated { get; }
		void Disable();
		void Enable();
		void Increase();
	}
}