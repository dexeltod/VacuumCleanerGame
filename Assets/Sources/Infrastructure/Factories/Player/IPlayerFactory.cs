using Cysharp.Threading.Tasks;
using Infrastructure.Services;
using InfrastructureInterfaces;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			IPlayerStatsService stats);
	}
}