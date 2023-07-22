using Cysharp.Threading.Tasks;
using Sources.Core;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			IPlayerStatsService stats);
	}
}