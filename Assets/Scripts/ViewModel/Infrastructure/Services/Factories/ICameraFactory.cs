using System.Threading.Tasks;
using Cinemachine;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface ICameraFactory : ICamera
	{
		Task<CinemachineVirtualCamera> CreateVirtualCamera();
	}
}