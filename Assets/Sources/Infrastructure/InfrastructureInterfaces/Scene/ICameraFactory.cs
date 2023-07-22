using System.Threading.Tasks;
using Cinemachine;

namespace Sources.Infrastructure.InfrastructureInterfaces.Scene
{
	public interface ICameraFactory : ICamera
	{
		Task<CinemachineVirtualCamera> CreateVirtualCamera();
	}
}