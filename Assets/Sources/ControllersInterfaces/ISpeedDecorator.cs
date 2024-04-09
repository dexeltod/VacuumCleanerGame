namespace Sources.ControllersInterfaces
{
	public interface ISpeedDecorator
	{
		void Increase();
		bool IsDecorated { get; }
	}
}