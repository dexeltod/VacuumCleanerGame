using System.Threading.Tasks;
using Cinemachine;

namespace Model.Infrastructure.Services.Factories
{
	public interface ICameraFactory : ICamera
	{
		Task<CinemachineVirtualCamera> CreateVirtualCamera();
	}
}