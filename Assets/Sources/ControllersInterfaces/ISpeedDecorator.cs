namespace Sources.ControllersInterfaces
{
	public interface ISpeedDecorator
	{
		void Increase();
		bool IsDecorated { get; }
		void Enable();
		void Disable();
	}
}