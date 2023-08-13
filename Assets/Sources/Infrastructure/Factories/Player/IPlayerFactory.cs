using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		void Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			IPlayerStatsService stats);
	}
}