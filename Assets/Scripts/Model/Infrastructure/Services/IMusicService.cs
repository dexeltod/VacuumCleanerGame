using Cysharp.Threading.Tasks;

namespace Model.Infrastructure.Services
{
	public interface IMusicService : IService
	{
		UniTask Set(string audioName);
		void Stop();
	}
}