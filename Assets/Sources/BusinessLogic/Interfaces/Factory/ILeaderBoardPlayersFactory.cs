using Cysharp.Threading.Tasks;
using Sources.PresentationInterfaces;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface ILeaderBoardPlayersFactory
	{
		UniTask Create(ILeaderBoardView leaderBoardView);
	}
}
