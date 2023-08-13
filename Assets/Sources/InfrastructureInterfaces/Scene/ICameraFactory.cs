using Cinemachine;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ICameraFactory : ICamera
	{
		CinemachineVirtualCamera CreateVirtualCamera();
	}
}