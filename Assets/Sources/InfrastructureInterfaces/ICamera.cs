namespace InfrastructureInterfaces
{
	public interface ICamera : IService
	{
		UnityEngine.Camera Camera { get; }
	}
}