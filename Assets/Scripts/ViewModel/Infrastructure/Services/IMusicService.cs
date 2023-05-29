using Cysharp.Threading.Tasks;

namespace ViewModel.Infrastructure.Services
{
	public interface IMusicService : IService
	{
		UniTask Set(string audioName);
		void Stop();
	}
}