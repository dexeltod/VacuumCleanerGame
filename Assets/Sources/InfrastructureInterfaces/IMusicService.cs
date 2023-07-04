using Cysharp.Threading.Tasks;

namespace InfrastructureInterfaces
{
	public interface IMusicService : IService
	{
		UniTask Set(string audioName);
		void Stop();
	}
}