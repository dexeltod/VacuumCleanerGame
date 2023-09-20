using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.ApplicationServicesInterfaces
{
	public interface ILeaderBoardService : IService
	{
		UniTask AddScore(int score);
	}
}