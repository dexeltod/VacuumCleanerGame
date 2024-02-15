using Joystick_Pack.Scripts.Base;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IPlayerFactory
	{
		UnityEngine.GameObject Player { get; }

		UnityEngine.GameObject Create(
			UnityEngine.GameObject initialPoint,
			Joystick joystick
		);
	}
}