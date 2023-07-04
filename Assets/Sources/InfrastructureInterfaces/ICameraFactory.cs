using System.Threading.Tasks;
using Cinemachine;

namespace InfrastructureInterfaces
{
	public interface ICameraFactory : ICamera
	{
		Task<CinemachineVirtualCamera> CreateVirtualCamera();
	}
}