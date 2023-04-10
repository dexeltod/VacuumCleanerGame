using Cysharp.Threading.Tasks;

namespace Model
{
	public interface IMusicService : IService
	{
		UniTask Set(string audioName);
		void Stop();
	}
}